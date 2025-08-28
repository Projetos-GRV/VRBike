from imutils.perspective import four_point_transform
from imutils import contours
import imutils
import cv2
import socket
import sys

# Variáveis globais para ROI
roi_start = None
roi_end = None
roi_selected = False
roi_created = False

frame_width = 0
frame_height = 0

temp_roi_start = (0, 0)
temp_roi_end = (0, 0)

# Função para mapear os segmentos ligados ao dígito
DIGITS_LOOKUP = {
	(1, 1, 1, 0, 1, 1, 1): 0,
	(0, 0, 1, 0, 0, 1, 0): 1,
	(0, 1, 1, 1, 1, 0, 1): 2,
	(0, 1, 1, 1, 0, 1, 1): 3,
	(1, 0, 1, 1, 0, 1, 0): 4,
	(1, 1, 0, 1, 0, 1, 1): 5,
	(1, 1, 0, 1, 1, 1, 1): 6,
	(0, 1, 1, 0, 0, 1, 0): 7,
	(1, 1, 1, 1, 1, 1, 1): 8,
	(1, 1, 1, 1, 0, 1, 1): 9
}

def select_roi(event, x, y, flags, param):
    global roi_start, roi_end, roi_selected, temp_roi_start, temp_roi_end

    if event == cv2.EVENT_RBUTTONDOWN:  # botão direito pressionado
        temp_roi_start = (x, y)
        roi_selected = True
    elif event == cv2.EVENT_MOUSEMOVE and roi_selected:
        temp_roi_end = (x, y)
    elif event == cv2.EVENT_RBUTTONUP:  # botão direito solto
        temp_roi_end = (x, y)
        roi_selected = False
        
        if (temp_roi_start[0] - temp_roi_end[0]) * (temp_roi_start[1] - temp_roi_end[1]) < 200:
            roi_start = (0,0)
            roi_end = (frame_width, frame_height)
        else:
            roi_start = temp_roi_start
            roi_end = temp_roi_end

        temp_roi_start = temp_roi_end = (0,0)



def recognize_digit(roi_digit):
    """Recebe uma imagem binária de um dígito e retorna o número reconhecido"""
    h, w = roi_digit.shape
    margin_y = int(h * 0.1)
    margin_x = int(w * 0.2)
    square_size = 3

    segments = [
        (margin_x, int(h * 0.25)),       #Esquerda-cima
        (w // 2, h - margin_y),          #topo
        (w - margin_x, int(h * 0.25)),   #Direita-cima
        (int(w * 0.5), int(h * 0.5)),    #centro
        (margin_x, int(h * 0.75)),       #esquerda-baixo
        (w - margin_x, int(h * 0.75)),   #direita-baixo
        (int(w * 0.5), margin_y)         #base
    ]

    on = []

    for (x, y) in segments:
        x1 = x - square_size
        x2 = x + square_size
        y1 = y - square_size
        y2 = y + square_size

        if x1 > w or x2 > w or y1 > h or y2 > h:
            continue

        seg_roi = roi_digit[y1:y2, x1:x2]
        if seg_roi.size == 0:
            on.append(0)
            continue

        area = (x2 - x1) * (y2 - y1)
        if area <= 0:
            on.append(0)
            continue

        total = cv2.countNonZero(seg_roi)
        on.append(1 if total / float(area) > 0.4 else 0)

        cv2.rectangle(roi_digit, (x1, y1), (x2, y2), (255,255,255), 2)

    on = tuple(on)
    
    return DIGITS_LOOKUP.get(on, "0")

def main():
    global roi_start, roi_end, roi_selected, temp_roi_start, temp_roi_end

    window_name = "Video"
    rotation_force = 1
    rotation_angle = 0

    # Escolher vídeo ou câmera
    video_path = 0#"velocimeter_3.mkv"  # coloque caminho do vídeo aqui se quiser
    cap = cv2.VideoCapture(0 if video_path=="" else video_path)

    frame_width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
    frame_height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))

    roi_start = (0,0)
    roi_end = (frame_width, frame_height)

    cv2.namedWindow(window_name)
    cv2.setMouseCallback(window_name, select_roi)

    server_ip = sys.argv[1]
    server_port = int(sys.argv[2])

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    while True:
        ret, frame = cap.read()
        if not ret:
            cap.set(cv2.CAP_PROP_POS_FRAMES, 0)  # reinicia vídeo
            continue

        # aplicar rotação no frame
        if rotation_angle != 0:
            frame = imutils.rotate(frame, rotation_angle)

        x1, y1 = roi_start
        x2, y2 = roi_end
        roi = frame[min(y1,y2):max(y1,y2), min(x1,x2):max(x1,x2)]

        gray = cv2.cvtColor(roi, cv2.COLOR_BGR2GRAY)
        
        thresh = cv2.threshold(gray, 50, 255, cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)[1]
        kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (1, 5))
        thresh = cv2.morphologyEx(thresh, cv2.MORPH_OPEN, kernel)

        h, w = thresh.shape
        mid = w // 2
        left_digit = thresh[:, :mid]
        right_digit = thresh[:, mid:]

        d1 = recognize_digit(left_digit)
        d2 = recognize_digit(right_digit)

        cv2.putText(frame, f"{d1}{d2}", (x1, y1-10), cv2.FONT_HERSHEY_SIMPLEX, 1.5, (0,255,0), 3)

        cv2.rectangle(frame, roi_start, roi_end, (0,255,0), 2)
        cv2.rectangle(frame, temp_roi_start, temp_roi_end, (255,255,0), 2)
        cv2.imshow(window_name, frame)
        cv2.imshow(window_name + " left", left_digit)
        cv2.imshow(window_name + " right", right_digit)

        message = f'{d1}{d2}'
        client_socket.sendto(message.encode(), (server_ip, server_port))

        key = cv2.waitKey(30) & 0xFF
        if key == 27:  # ESC para sair
            break
        elif key == ord('p'):
            input()
        elif key == ord('q'):  # rotaciona anti-horário
            rotation_angle -= rotation_force
        elif key == ord('w'):  # rotaciona horário
            rotation_angle += rotation_force
        elif key == ord('r'):  # reseta rotação
            rotation_angle = 0

    cap.release()
    cv2.destroyAllWindows()


if __name__ == '__main__':
    main()