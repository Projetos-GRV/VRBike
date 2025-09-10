using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CustomUDPListener
{
    public class CustomUDPDataListener
    {
        private readonly int listenPortAngle;

        private UdpClient angleClient;
        private IPEndPoint angleEP;

        private Thread angleThread;

        public string data; // angle;speed

        public CustomUDPDataListener(int listenPortAngle)
        {
            this.listenPortAngle = listenPortAngle;
            this.data = "0;0";
        }

        public string GetData() { return this.data; }

        public void Halt()
        {
            this.angleClient.Close();
        }

        public bool Init()
        {
            bool success = false;
            try
            {
                //this.speedClient = new UdpClient(listenPortSpeed);

                this.angleClient = new UdpClient(listenPortAngle);

                //this.speedEP = new IPEndPoint(IPAddress.Any, listenPortSpeed);
                this.angleEP = new IPEndPoint(IPAddress.Any, listenPortAngle);

                success = true;

            }
            catch (System.Exception e)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.Log(e);
                }
                success = false;
            }

            if (success)
            {
                //this.speedThread = new Thread(() => Run(this.speedClient, this.speedEP, out this.speed));
                //this.speedThread.Start();
                this.angleThread = new Thread(() => Run(this.angleClient, this.angleEP, out this.data));
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
                }
                catch (SocketException)
                { }
            }
        }
    }
}