# Virtual Reality Bicycle Simulator (Developing)

This project is a Virtual Reality application that simulates riding a bicycle using real-world sensors and a 3D environment built in Unity for the Meta Quest 3.

## How it Works

- Handlebar Control:
A potentiometer is attached to the real bicycle’s handlebar axis using a custom 3D-printed mount. It captures steering movements and sends them to the virtual bicycle.

- Speed Tracking:
A bicycle speedometer sensor with a magnet detects when the wheel completes one rotation. From this, the system calculates the current speed.

- Data Processing:
Both steering and speed data are collected by a notebook, formatted, and transmitted to the Meta Quest 3 via Sockets.

- Virtual Simulation:
Inside the VR headset, a Unity application simulates the 3D bicycle, updating its rotation and movement based on real sensor inputs.

![alt text](https://github.com/Projetos-GRV/VRBike_Temp/blob/Dev-Vinicius/imgs/bike_1.jpg)
![alt text](https://github.com/Projetos-GRV/VRBike_Temp/blob/Dev-Vinicius/imgs/bike_2.jpg)
![alt text](https://github.com/Projetos-GRV/VRBike_Temp/blob/Dev-Vinicius/imgs/bike_3.jpg)
![alt text](https://github.com/Projetos-GRV/VRBike_Temp/blob/Dev-Vinicius/imgs/app_1.png)
![alt text](https://github.com/Projetos-GRV/VRBike_Temp/blob/Dev-Vinicius/imgs/app_2.png)
## Features

### Procedural City Generation: 

- An infinite 3D city is generated at runtime, allowing players to ride endlessly through the environment.

### Gameplay Mechanics:

- Collect coins scattered throughout the city.

- Pick up speed multiplier items to temporarily boost base speed.

### Goal

The project combines hardware integration, real-time data streaming, and VR development to create a fun and immersive experience of riding a bicycle inside a virtual world.


# Demo

Link: [https://youtu.be/GAMwGyg6p1Y](https://youtu.be/GAMwGyg6p1Y)

# Authors

- Mauricio Luz (mauricio.luz22@edu.pucrs.br)
- Piedro Rockembach Nunes (piedro.n@edu.pucrs.br)
- Vinicius Chrisosthemos Teixeira (Vinicius.Teixeira99@edu.pucrs.br)
- Henrique Gabriel Cabral
- Thiago Almeida Mocinho
- Valdemir Fernando Braga Pereira
