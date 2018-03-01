using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public string DatosPC { get; set; }

        private const string URLASPZERO = "http://192.168.13.78:22742/api/services/app/RelevamientoDIService/SincronizacionRelevamiento"

        public string HelloWorld(string name)
        {
            return "It's a helluva World, " + name;
        }

        public decimal Add(decimal number1, decimal number2)
        {
            return number1 + number2;
        }

        public bool Validar()
        {
            return true;
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


}
