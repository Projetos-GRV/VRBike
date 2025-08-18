using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem; // provavelmente sera desnecessario.... ja que o movimento não virá de um controle.......

[Serializable]
public class BicycleExternalMovementSource : MonoBehaviour, IBicycleMovementSource
{
    public float maxHandlebarAngle = 80.0f;

    private const int baudRate = 9600;      // tenho nem ideia
    private const string portName = "COM3";      // Porta serial
    private const float angleZero = 500.0f;      // ponto de referencia para o angulo 0 do guidao
    private SerialPort handlebarSensor = null; 

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
        {
            this.handlebarSensor = new SerialPort(portName, baudRate);
            handlebarSensor.Open();
            handlebarSensor.ReadTimeout = 100;
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
    {
        if (this.handlebarSensor != null && this.handlebarSensor.IsOpen)
        {
            this.handlebarSensor.Close();
            Debug.Log("Porta serial fechada.");
        }
    }

    // provavelmente sera aqui onde os movimentos da bicicleta serao buscados e atualizados
    void Update()
    {
        if (this.handlebarSensor == null || !this.handlebarSensor.IsOpen)
        {
            return;
        }

        this.handlebarRotation = (ReadHandlebarSensor());
        this.speed = 3;

    }

    private float ReadHandlebarSensor()
    {
        int dir = 0;
        float angle = 0;
        try
        {
            string input = this.handlebarSensor.ReadLine();
            int value = int.Parse(Regex.Replace(input.Trim(), "[^0-9]", ""));
            dir = value / 100;

            // float t = (dir - angleZero) / angleZero;
            float t = (dir) / (2 * angleZero); // soh sei que parece funcionar
            angle = -Mathf.Lerp(-maxHandlebarAngle, maxHandlebarAngle, t); // ta invertido
            //angle = Map(-80, 80, 0, 1, t);
            Debug.Log(angle);
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
        return angle;
    }

    // alternativa pro Lerp
    //private float Map(float from, float to, float from2, float to2, float input)
    //{
    //    if (input <= from2)
    //    {
    //        return from;
    //    } else if (input >= to2)
    //    {
    //        return to;
    //    }
    //    return (to - from) * ((input - from2) / (to2 - from2)) + from;
    //}

    // essa funcao pode ser mais necessaria do que se pensava se os botoes de arcade forem utilizados
    void OnMove(InputValue movementValue)
    {
    }
}
