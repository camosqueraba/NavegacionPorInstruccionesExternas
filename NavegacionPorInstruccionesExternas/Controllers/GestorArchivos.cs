#region usings
using System;
using System.Globalization;
using System.IO;
#endregion

namespace RPASimulatorbank_CBZ.Controllers
{
    public class GestorArchivos
    {
        #region metodo convertir base 64 a archivo
        public static string ConvertirBase64AArchivo(string strBase64, string nombre, string extension)
        {
            try
            {
                string ruta = null;
                if (!string.IsNullOrEmpty(strBase64))
                {
                    string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";                    
                    ruta = Path.Combine(download, $"{nombre}.{extension}");
                    Byte[] bytes = Convert.FromBase64String(strBase64);
                    File.WriteAllBytes(ruta, bytes);
                    LOGRobotica.Controllers.LogApplication.LogWrite("GestorArchivos -> ConvertirBase64AArchivo :" + $"Se crea archivo en: {ruta}");
                }

                return ruta;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("GestorArchivos -> ConvertirArchivoABase64 -> Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }
        #endregion
    }
}