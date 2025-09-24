using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    [Header("Configurações")]
    public int listenPort = 7777; // Porta onde o socket vai escutar

    [Header("Status")]
    public string lastMessage = "";   // Última mensagem recebida
    public bool messageReady = false; // Flag indicando se há mensagem nova

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool running = false;

    void Start()
    {
        StartReceiver();
    }

    void OnApplicationQuit()
    {
        StopReceiver();
    }

    /// <summary>
    /// Inicializa o socket UDP e a thread de escuta.
    /// </summary>
    public void StartReceiver()
    {
        try
        {
            udpClient = new UdpClient(listenPort);
            udpClient.Client.ReceiveTimeout = 0; // Não expira
            running = true;

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log($"[UDP] Escutando na porta {listenPort}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[UDP] Erro ao iniciar: {ex.Message}");
        }
    }

    /// <summary>
    /// Fecha o socket e encerra a thread.
    /// </summary>
    public void StopReceiver()
    {
        running = false;

        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }

        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }

    /// <summary>
    /// Thread para receber dados via UDP.
    /// </summary>
    private void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (running)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

                lastMessage = message;
                messageReady = true;

                Debug.Log($"[UDP] Mensagem recebida de {remoteEndPoint}: {message}");
            }
            catch (SocketException)
            {
                // Timeout ou socket fechado — ignorar
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[UDP] Erro: {ex.Message}");
            }
        }
    }
}
