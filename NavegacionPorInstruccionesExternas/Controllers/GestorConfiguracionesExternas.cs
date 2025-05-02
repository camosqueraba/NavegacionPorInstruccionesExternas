#region
using Newtonsoft.Json;
using NavegacionPorInstruccionesExternas.Model;
using System;
using System.Configuration;
using System.IO;
using NavegacionPorInstruccionesExternas.DTOs;
using RPASimulatorbank_CBZ.Controllers;
#endregion

namespace NavegacionPorInstruccionesExternas.Controller
{
    public class GestorConfiguracionesExternas
    {
        #region propiedades de clase
        private readonly string ArchivoConfiguracionesJson = ConfigurationManager.AppSettings["ArchivoConfiguracionesJson"];
        #endregion

        public GestorConfiguracionesExternas()
        {
                //GestorArchivos.ObtenerArchivoByUrl()
        }

        #region leer configuraciones pdf
        public  ConfiguracionesNavegacion LeerConfiguracionesNavegacion()
        {
            try
            {
                string json;
                ConfiguracionesNavegacion configuracionesNavegacion = null;

                using (StreamReader sr = new StreamReader(ArchivoConfiguracionesJson))
                {
                    json = sr.ReadToEnd();
                    ConfiguracionesExternas configuraciones = JsonConvert.DeserializeObject<ConfiguracionesExternas>(json);
                    if (configuraciones != null && configuraciones.ConfiguracionesNavegacion != null)
                    {
                        configuracionesNavegacion = configuraciones.ConfiguracionesNavegacion;
                    }
                }

                return configuracionesNavegacion;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("GestorConfiguracionesExternas -> LeerConfiguracionesNavegacion " + "Exception: " + ex.Message);
                return null;
            }
        }
        #endregion
    }
}