using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    [ComSourceInterfaces(typeof(IEventos))]
    public class Examples
    {
        public string NumeroSuscriptor { get; set; }
        public string messageJWT = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ";
        public string Path_Server;
        private string Ip_Name_Server;

        private const string URLASPZERO = "http://192.168.13.78:22742/api/services/app/RelevamientoDIService/SincronizacionRelevamiento";

        // Receiving byte array  
        byte[] bytes = new byte[1024];
        Socket senderSock;
        
        const int DEFAULT_PORT = 804;

        public Error Errores { get; set; }
        public Response Respuesta { get; set; }
        private List<string> Mensajes { get; set; }

        public BackgroundWorker oWorkerBurn;
        public delegate void FinalizoHandler(bool lExito);
        public event FinalizoHandler finalizo;

        public Examples()
        {
            Errores = new Error();
            Respuesta = new Response();
            Mensajes = new List<string>();

            oWorkerBurn = new BackgroundWorker();
            oWorkerBurn.WorkerSupportsCancellation = true;
            oWorkerBurn.WorkerReportsProgress = true;
            oWorkerBurn.DoWork += OWorkerBurn_DoWork;
            oWorkerBurn.RunWorkerCompleted += OWorkerBurn_RunWorkerCompleted;

        }

        private void OWorkerBurn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ConectToServer();
            }
            catch (Exception ex)
            {
                Errores = new Error()
                {
                    tieneError = true,
                    MensajesError = ex.Message
                };
            }
        }

        public bool RunService()
        {
            try
            {
                // RESETEAR LA RESPUESTA Y ERRORES!!!!
                oWorkerBurn.RunWorkerAsync();
                return true;
            }
            catch (Exception ex)
            {
                Errores = new Error()
                {
                    tieneError = true,
                    MensajesError = ex.Message
                };

                return false;
            }
        }


        private void ConectToServer()
        {

            try
            {
                //1- Descubrir IP o Nombre PC
                GetIpNameServer();
                //2- Conectamos a Server
                Connet();
                //3- Envio del mensaje 
                Send();
                //4- Liberar la conexión
                Disconnect();
                //5- Todo Ok
                if (Mensajes.Count>0)
                {
                    Respuesta.MensajeConexion = Mensajes.ToArray<string>();
                }
            }
            catch (Exception ex)
            {
                Errores = new Error()
                {
                    tieneError = true,
                    MensajesError = ex.Message
                };
            }
        }

        private void GetIpNameServer()
        {
            if (String.IsNullOrWhiteSpace(Path_Server))
            {
                throw new Exception("La ruta es nula o son espacios en blanco.");
            }

            if (!Path.IsPathRooted(Path_Server))
            {
                throw new Exception(
                    string.Format("La ruta '{0}' was not a rooted path and GetDriveLetter does not support relative paths."
                    , Path_Server)
                    );
            }

            if (Path_Server.StartsWith(@"\\"))
            {
                //throw new ArgumentException("A UNC path was passed to GetDriveLetter");
                var tmpPathServer = Path_Server.Substring(2);//sacamos los \\ iniciales
                int ponint = tmpPathServer.IndexOf('\\'); //buscamos la primera barra donde va separa el resto de la ruta
                Ip_Name_Server = tmpPathServer.Substring(0, ponint); // Retornamos la IP
            }
            else
            {
                Ip_Name_Server = Mapped.GetIpFromPath(Path_Server);
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
                IPHostEntry ipHost = Dns.GetHostEntry(Ip_Name_Server);

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

                Mensajes.Add("Socket concectado a: " + senderSock.RemoteEndPoint.ToString());
            }
            catch (Exception exc) { throw new Exception("No se puedo conectar al servidor\nMensaje del Servidor: " + exc.Message); }
        }

        private void Send()
        {
            try
            {
                // Sending message 
                //<Client Quit> is the sign for end of data 
                string theMessageToSend = messageJWT + "<Client Quit>";
                byte[] msg = Encoding.Unicode.GetBytes(theMessageToSend);

                // Sends data to a connected Socket. 
                int bytesSend = senderSock.Send(msg);

                Respuesta.MensajeServidor = ReceiveDataFromServer();

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

                return "El servidor respondio: " + theMessageToReceive;
            }
            catch (Exception) { throw new Exception("Error al recibir mensaje del servidor"); }
        }

        private void Disconnect()
        {
            try
            {
                // Disables sends and receives on a Socket. 
                senderSock.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources 
                senderSock.Close();

                Mensajes.Add("Se cerro la conexión");
            }
            catch (Exception) { throw new Exception("Error al desconectar"); }
        }


        private void OWorkerBurn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            onFinalizo(sender, e);
        }

        public void onFinalizo(object sender, RunWorkerCompletedEventArgs e)
        {
            if (finalizo != null)
            {
                this.finalizo(true);
            }
            else
            {
                this.finalizo(false);
            }
        }



        public bool ValidarConexion()
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
                // Disables sends and receives on a Socket. 
               senderSock.Shutdown(SocketShutdown.Both);
                //Closes the Socket connection and releases all resources 
                senderSock.Close();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string HelloWorld(string name)
        {
            return "It's a helluva World, " + name.ToUpper();
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


    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("InteropExamples.Response")]
    public class Response
    {
        public string[] MensajeConexion { get; set; }
        public string MensajeServidor { get; set; }
    }
}
