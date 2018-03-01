using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace cSharpChat
{
    public class OSCoreServer
    {
        const string DEFAULT_SERVER = "localhost";
        const int DEFAULT_PORT = 804;

        //SERVER SOCKET STUFF
        Socket serverSocket;
        SocketInformation serverSocketInfo;


        //Client socket stuff
        Socket clientSocket;
        SocketInformation clientSocketInfo;


        public string Startup()
        {
            //The chat server always starts up on the localhost, using the default port 
            IPHostEntry hostInfo = Dns.GetHostEntry(DEFAULT_SERVER);
            IPAddress serverAddr = hostInfo.AddressList.First(f=>f.AddressFamily != AddressFamily.InterNetworkV6);
            var serverEndPoint = new IPEndPoint(serverAddr, DEFAULT_PORT);

            // Create a listener socket and bind it to the endpoint 
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(serverEndPoint);
           
            return serverSocket.LocalEndPoint.ToString();
        }

        public string Listen()
        {
            int backlog = 0;
            try
            {
                serverSocket.Listen(backlog);
                return "Server listening";
            }
            catch (Exception ex)
            {
                return "Faild to listen" + ex.Message;
            }
        }


        public bool SendData(string textData)
        {
            if (string.IsNullOrEmpty(textData))
            {
                return false;
            }

            if (textData.Trim().ToLower() == "exit")
            {
                return true;
            }

            // The chat client always starts up on the localhost, using the default port 
            IPHostEntry hostInfo = Dns.GetHostEntry(DEFAULT_SERVER);
            IPAddress serverAddr = hostInfo.AddressList.First(f => f.AddressFamily != AddressFamily.InterNetworkV6);
            var clientEndPoint = new IPEndPoint(serverAddr, DEFAULT_PORT);

            // Create a client socket and connect it to the endpoint 
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(clientEndPoint);

            byte[] byData = Encoding.ASCII.GetBytes(textData);
            clientSocket.Send(byData);
            clientSocket.Close();

            return false;

        }

        public string ReceiveData()
        {

            var receiveSocket = serverSocket.Accept();

            byte[] buffer = new byte[256];
            var bytesrecd = receiveSocket.Receive(buffer);

            receiveSocket.Close();
            var text = Encoding.UTF8.GetString(buffer);
            
            return text ;

        }


    }
}
