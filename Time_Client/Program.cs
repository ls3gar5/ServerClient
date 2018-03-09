using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Time_Client
{
    class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            Console.Title = "Cliente";

            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void LoopConnect()
        {
            int attemps = 0;
            while (!_clientSocket.Connected)
            {
                try
                {
                    attemps++;

                    _clientSocket.Connect(IPAddress.Loopback, 804);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection attemps:" + attemps.ToString());
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }

        private static void SendLoop()
        {
            while (true)
            {
                Thread.Sleep(1000);
                //Console.Write("Enter a request: ");
                //string req = Console.ReadLine();
                byte[] buffer = Encoding.Unicode.GetBytes("Get Time");
                _clientSocket.Send(buffer);

                byte[] recievedBuf = new byte[1024];
                int rec = _clientSocket.Receive(recievedBuf);
                byte[] data = new byte[rec];
                Array.Copy(recievedBuf, data, rec);

                Console.WriteLine("Recieved: " + Encoding.Unicode.GetString(data));

            }
        }
    }
}
