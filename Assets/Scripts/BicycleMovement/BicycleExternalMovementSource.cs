using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
/*
using System.IO.Ports;*/
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento n�o vir� de um controle.......

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public float maxHandlebarAngle = 80.0f;
    public float wheelRadius = 0;

    private const int baudRate = 9600;      // tenho nem ideia
    private const string portName = "COM3";      // Porta serial
    private const float angleZero = 500.0f;      // ponto de referencia para o angulo 0 do guidao
    //private SerialPort handlebarSensor = null; 

    private float speed;
    private float handlebarRotation;
    private Vector2 direction;

    public float GetHandlebarRotation() { return this.handlebarRotation; }
    public Vector2 GetFrontWheelDirection() { return this.direction; }
    public float GetSpeed() { return this.speed; }

    void Start()
    {
        this.speed = 0;
        this.handlebarRotation = 0;
        this.direction = Vector2.zero;

        try
        {/*
            this.handlebarSensor = new SerialPort(portName, baudRate);
            handlebarSensor.Open();
            handlebarSensor.ReadTimeout = 100;*/
            if (Debug.isDebugBuild)
            {
                Debug.Log("Porta serial aberta em: " + portName);
            }
        } catch (System.Exception e)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log("Nao foi possivel abrir a porta serial em " + portName + ": " + e.Message);
            }
        }
    }

    void OnApplicationQuit()
    {/*
        if (this.handlebarSensor != null && this.handlebarSensor.IsOpen)
        {
            this.handlebarSensor.Close();
            if (Debug.isDebugBuild)
            {
                Debug.Log("Porta serial fechada.");
            }
        }*/
    }

    // para teste com o oculus de realidade virtual caso nao seja possivel pegar a velocidade pela bicicleta
    public void SetSpeed(float speed)
    {
        if (speed == 0)
        {
            this.speed = 0;
        }
        else
        {
            this.speed += speed;
        }
    }

    // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados
    void Update()
    {/*
        if (this.handlebarSensor == null || !this.handlebarSensor.IsOpen)
        {
            return;
        }
        */
        ReadSensors();
        if (Debug.isDebugBuild)
        {
            //Debug.Log(this.handlebarRotation);
            //Debug.Log(this.speed);
        }
        
        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(this.handlebarRotation, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;
    }

    private float ReadSensors()
    {
        float angle = 0;/*
        float speed = 0;
        try
        {
            string[] inputs = Regex.Replace(this.handlebarSensor.ReadLine(), "[^0-9;]", "").Split(";");
         
            float speedIn = float.Parse(inputs[0], CultureInfo.InvariantCulture);
            float angleIn = float.Parse(inputs[1], CultureInfo.InvariantCulture);

            float t = angleIn;
            angle = Map(-this.maxHandlebarAngle, this.maxHandlebarAngle, 0, 1023, t); // t NAO precisa mais ser invertido
            speed = speedIn;
        }
        catch (System.TimeoutException e)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log(e.Message);
            }
        }
        catch (System.Exception e)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log(e.Message);
            }
        }
        this.handlebarRotation = angle;
        this.speed = speed * 5.0f;*/
        return angle;
    }

    // alternativa pro Lerp. Retirada dos forums da Unity
    // argumentos from e to == conversao destino (minimo e maximo, respectivamente)
    // argumentos from2 e to2 == minimo e maximo que input pode ser
    // argumento input == valor entre [from2, to2]
    private float Map(float from, float to, float from2, float to2, float input)
    {
        if (input <= from2)
        {
            return from;
        }
        else if (input >= to2)
        {
            return to;
        }
        return (to - from) * ((input - from2) / (to2 - from2)) + from;
    }
}
