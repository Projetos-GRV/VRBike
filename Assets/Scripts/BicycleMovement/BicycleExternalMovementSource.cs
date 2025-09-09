using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento n�o vir� de um controle.......

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public float maxHandlebarAngle = 80.0f;

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

    // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados
    void Update()
    {/*
        if (this.handlebarSensor == null || !this.handlebarSensor.IsOpen)
        {
            return;
        }

        this.handlebarRotation = ReadHandlebarSensor();
        this.speed = ReadSpeedSensor();
        
        Vector3 tmp = Vector3.forward;
        tmp = Quaternion.AngleAxis(this.handlebarRotation, Vector3.up) * tmp;
        this.direction.x = tmp.x;
        this.direction.y = tmp.z;*/
    }

    private float ReadHandlebarSensor()
    {
        int dir = 0;
        float angle = 0;/*
        try
        {
            string input = this.handlebarSensor.ReadLine();
            int value = int.Parse(Regex.Replace(input.Trim(), "[^0-9]", ""));
            dir = value / 100;

            float t = (dir - angleZero) / angleZero;
            //Debug.Log(t);
            //float t = (dir) / (2 * angleZero); // soh sei que parece funcionar
            //angle = -Mathf.Lerp(-maxHandlebarAngle, maxHandlebarAngle, t); // ta invertido
            angle = Map(-80.0f, 80.0f, -0.65f, 0.65f, -t); // t precisa ser invertido
            //Debug.Log(angle);
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
        }*/
        return angle;
    }

    private float ReadSpeedSensor()
    {
        return 0.0f;
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
