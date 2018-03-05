using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropExamples
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("InteropExamples.Examples")]
    public class Examples
    {
        public string NumeroSuscriptor { get; set; }
        public const string messageJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ";
        private const string URLASPZERO = "http://192.168.13.78:22742/api/services/app/RelevamientoDIService/SincronizacionRelevamiento";
        // Receiving byte array  
        byte[] bytes = new byte[1024];
        Socket senderSock;
        const string DEFAULT_SERVER = "localhost";
        const int DEFAULT_PORT = 804;

        public Error MyError { get; set; }


        public string HelloWorld(string name)
        {
            return "It's a helluva World, " + name.ToUpper();
        }

        public void ConectToServer()
        {

            try
            {
                //1- Conectamos a Server
                Connet();
                //2- Envio del mensaje 
                Send();
                //3- Liberar la conexión
                Disconnect();
                //4- Todo Ok
                MyError = new Error()
                {
                    tieneError = false,
                    MensajesError = null
                };
            }
            catch (Exception ex)
            {
                MyError = new Error()
                {
                    tieneError = true,
                    MensajesError = ex.Message
                };
            }
        }


        private void Connet()
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
                
                //return "Socket connected to " + senderSock.RemoteEndPoint.ToString();
            }
            catch (Exception exc) { throw new Exception("No se puedo conectar al servidor"); }
        }


        private string Send()
        {
            try
            {
                // Sending message 
                //<Client Quit> is the sign for end of data 
                string theMessageToSend = messageJWT + "<Client Quit>";
                byte[] msg = Encoding.Unicode.GetBytes(theMessageToSend);

                // Sends data to a connected Socket. 
                int bytesSend = senderSock.Send(msg);

                return ReceiveDataFromServer();

            }
            catch (Exception) { throw new Exception("Error al enviar mensaje"); }
        }

        private string ReceiveDataFromServer()
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

                return "The server reply: " + theMessageToReceive;
            }
            catch (Exception) { throw new Exception("Error al recibir mensaje del servidor"); }
        }

        private string Disconnect()
        {
            try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                return "Se cerro la conexión";
            }
            catch (Exception) { throw new Exception("Error al desconectar"); }
        }


        //public bool CallPOST()
        //{
        //    List<int> idsRelevamiento = new List<int>();
        //    List<int> idsArchivos = new List<int>();
        //    idsRelevamiento.Add(71753);
        //    idsRelevamiento.Add(71751);
        //    idsRelevamiento.Add(71752);
        //    idsArchivos.Add(5);
        //    idsArchivos.Add(6);
        //    idsArchivos.Add(7);

        //    var Text = string.Empty;
        //    var values = string.Format("[{0}], [{1}]", idsRelevamiento, idsArchivos);
        //    var stdObj = new SicronizacionDto()
        //    {
        //        idsArchivos = idsArchivos.ToList(),
        //        idsRelevamientos = idsRelevamiento.ToList()
        //    };

        //    var serilizacion = Newtonsoft.Json.JsonConvert.SerializeObject(stdObj);

        //    var bytes = Encoding.ASCII.GetBytes(serilizacion);

        //    var bytes = Encoding.ASCII.GetBytes(values);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLASPZERO);
        //    request.Timeout = 100000;
        //    request.Method = "POST";
        //    request.ContentType = "application/json; charset=utf-8";
        //    request.Accept = "application/json";
        //    request.ContentLength = bytes.Length;
        //    request.Credentials = new NetworkCredential("User", "Pass", "Dom");


        //    using (var requestStream = request.GetRequestStream())
        //    {
        //        requestStream.Write(bytes, 0, bytes.Length);
        //    }
        //    var response = (HttpWebResponse)request.GetResponse();

        //    if (response.StatusCode == HttpStatusCode.OK)
        //        Text = "Update completed";
        //    else
        //        Text = "Error in update";
        //}
    }


    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("InteropExamples.Error")]
    public class Error
    {
        public bool tieneError { get; set; }
        public string MensajesError { get; set; }
    }
}
