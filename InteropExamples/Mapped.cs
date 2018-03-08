using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropExamples
{
    public static class Mapped
    {
        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WNetGetConnection(
            [MarshalAs(UnmanagedType.LPTStr)] string localName,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder remoteName,
            ref int length);

        public static string GetIpFromPath(string originalPath)
        {
            try
            {
                originalPath = Directory.GetDirectoryRoot(originalPath).Replace(Path.DirectorySeparatorChar.ToString(), "");
                string pathServer = string.Empty;

                StringBuilder sb = new StringBuilder(512);
                int size = sb.Capacity;
                char c = originalPath[0];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    //Uso de API para ver las unidad de red
                    WNetGetConnection(originalPath.Substring(0, 2), sb, ref size);

                    if (sb.Length > 0)
                    {

                        pathServer = sb.ToString().Substring(2);//sacamos los \\ iniciales
                        int ponint = pathServer.IndexOf('\\'); //buscamos la primera barra donde va separa el resto de la ruta
                        pathServer = pathServer.Substring(0, ponint); // Retornamos la IP
                    }
                    else
                    {
                        throw new Exception("Error al detertar la IP del servidor");
                    }
                }
                else
                {
                    throw new Exception("Verifique la letra asignada a la unidad de red, la misma no esta entre la A y la Z");
                }

                return pathServer;
            }
            catch (Exception ex)
            {

                throw new Exception("No se pudo utilizar la API de Windows 'npr.dll' para descubrir la IP del servidor.\n"
                    + "Mensaje de error: " + ex.Message);

            }
        }
    }
}
