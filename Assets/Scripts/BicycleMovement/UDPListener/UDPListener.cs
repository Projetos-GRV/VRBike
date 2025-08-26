using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UDPListener
{
    public class UDPDataListener
    {
        private readonly int listenPortSpeed;
        private readonly int listenPortAngle;

        private UdpClient speedClient;
        private UdpClient angleClient;
        private IPEndPoint speedEP;
        private IPEndPoint angleEP;

        private Thread speedThread;
        private Thread angleThread;

        private string angles; // x;y;z
        private string speed;

        private float angleTime;
        private Stopwatch stopwatch;

        public UDPDataListener(int listenPortSpeed, int listenPortAngle)
        {
            this.angles = "0;0;0";
            this.speed = "0";
            this.angleTime = 0;
            this.stopwatch = new Stopwatch();
            this.listenPortSpeed = listenPortSpeed;
            this.listenPortAngle = listenPortAngle;
        }

        public string GetAngleData() { return this.angles; }
        public string GetSpeedData() { return this.speed; }
        public float GetAngleTime() { return this.angleTime; }

        public void Halt()
        {
            this.speedClient.Close();
            this.angleClient.Close();
        }

        public bool Init()
        {
            bool success = false;
            try
            {
                this.speedClient = new UdpClient(listenPortSpeed);
                this.angleClient = new UdpClient(listenPortAngle);

                this.speedEP = new IPEndPoint(IPAddress.Any, listenPortSpeed);
                this.angleEP = new IPEndPoint(IPAddress.Any, listenPortAngle);

                success = true;

            }
            catch (System.Exception)
            {
                //if (Debug.isDebugBuild)
                //{
                //    Debug.Log(e);
                //}
                success = false;
            }

            if (success) {
                this.speedThread = new Thread(() => Run(this.speedClient, this.speedEP, out this.speed));
                this.speedThread.Start();
                this.stopwatch.Start();
                this.angleThread = new Thread(() => Run(this.angleClient, this.angleEP, out this.angles));
                this.angleThread.Start();
            }
            return success;
        }

        private void Run(UdpClient listener, IPEndPoint EP, out string outputStr)
        {
            while (true)
            {
                try
                {
                    byte[] bytes = listener.Receive(ref EP);
                    outputStr = Encoding.ASCII.GetString(bytes, 0, bytes.Length).Trim();
                    this.stopwatch.Stop();
                    long elapsed = this.stopwatch.ElapsedMilliseconds;
                    this.angleTime = elapsed;
                    this.stopwatch.Restart();
                    this.stopwatch.Start();
                }
                catch (SocketException)
                {}
            }
        }
    }
}