using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cSharpChat
{
    class Program
    {
        static void Main(string[] args)
        {

            //string remoteServerName = server.ParseArgs(args); 

            var server = new OSCoreServer();

            string serverInfo = server.Startup();
            Console.WriteLine("Server started at:" + serverInfo);

            serverInfo = server.Listen();
            Console.WriteLine(serverInfo);

            string datatosend = Console.ReadLine();
            server.SendData(datatosend);

            serverInfo = server.ReceiveData();
            Console.WriteLine(serverInfo);

            Console.ReadLine();
            //exit 
            ;
        }
    }
}
