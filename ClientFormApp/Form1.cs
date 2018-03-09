using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InteropExamples;

namespace ClientFormApp
{
    public partial class Form1 : Form
    {
        // Receiving byte array  
        byte[] bytes = new byte[1024];
        Socket senderSock;
        const string DEFAULT_SERVER = "localhost";
        const int DEFAULT_PORT = 804;
        string messageJWT = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Connet_Button_Click(object sender, EventArgs e)
        {
            try
            {
                // Create one SocketPermission for socket access restrictions 
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Connect,    // Connection permission 
                    TransportType.Tcp,        // Defines transport types 
                    "",                       // Gets the IP addresses 
                    SocketPermission.AllPorts // All ports 
                    );

                // Ensures the code to have permission to access a Socket 
                permission.Demand();

                // Resolves a host name to an IPHostEntry instance            
                IPHostEntry ipHost = Dns.GetHostEntry("");

                // Gets first IP address associated with a localhost 
                IPAddress ipAddr = ipHost.AddressList.First(f => f.AddressFamily != AddressFamily.InterNetworkV6);

                // Creates a network endpoint 
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, DEFAULT_PORT);

                // Create one Socket object to setup Tcp connection 
                senderSock = new Socket(
                    ipAddr.AddressFamily,// Specifies the addressing scheme 
                    SocketType.Stream,   // The type of socket  
                    ProtocolType.Tcp     // Specifies the protocols  
                    );

                senderSock.NoDelay = false;   // Using the Nagle algorithm 

                // Establishes a connection to a remote host 
                senderSock.Connect(ipEndPoint);
                tbStatus.Text = "Socket connected to " + senderSock.RemoteEndPoint.ToString();
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void Send_Button_Click(object sender, EventArgs e)
        {
            try
            {
                // Sending message 
                //<Client Quit> is the sign for end of data 
                string theMessageToSend  = messageJWT + "<Client Quit>";
                byte[] msg = Encoding.Unicode.GetBytes(theMessageToSend );

                // Sends data to a connected Socket. 
                int bytesSend = senderSock.Send(msg);

                ReceiveDataFromServer();

            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void ReceiveDataFromServer()
        {
            try
            {
                // Receives data from a bound Socket. 
                int bytesRec = senderSock.Receive(bytes);

                // Converts byte array to string 
                String theMessageToReceive = Encoding.Unicode.GetString(bytes, 0, bytesRec);

                // Continues to read the data till data isn't available 
                while (senderSock.Available > 0)
                {
                    bytesRec = senderSock.Receive(bytes);
                    theMessageToReceive += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                }

                txtMsgRecividoServidor.Text = "The server reply: " + theMessageToReceive;
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        private void Disconnect_Button_Click(object sender, EventArgs e)
        {
            try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                tbStatus.Text = "Se cerro la conexión";
            }
            catch (Exception exc) { MessageBox.Show(exc.ToString()); }
        }

        Examples client = new Examples();

        private void Form1_Load(object sender, EventArgs e)
        {
           // creates a number between 1 and 12
            int dice = rnd.Next(1, 7);   // creates a number between 1 and 6
            int card = rnd.Next(52);
            ///Opciones \\WIN7PROX64\HolistorW
            ///Y:\Whapp (referencia con el nombre de la pc)
            ///Z:\Whapp (referencia con )
            client.Path_Server = @"\\DESA01\";
            client.finalizo += Pp_finalizo;
            client.RunService();
        }

        private void Pp_finalizo(bool lExito)
        {
            if (client.Errores.tieneError)
            {
                var mensajes = client.Errores.MensajesError;
            }

            var msg = client.Respuesta.MensajeConexion;
            var msg2 = client.Respuesta.MensajeServidor;
        }
    }
}
