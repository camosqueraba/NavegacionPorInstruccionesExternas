using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas.Controllers
{
    public class GlobalVars
    {
        #region Global Variables        
        public static string LOGs_GUID { get; set; } = Guid.NewGuid().ToString("N");
        public static bool OrderCerrar { get; set; }
        public static string RutaDescargaCarta { get; set; }

        #endregion

        #region Datos Maquina/ BOT
        public static string UsuarioRed { get; set; }
        public static string NombreMaquina { get; set; }

        public static readonly string NameRPA = ConfigurationManager.AppSettings["NameRPA"];
        public static readonly string VersionProject = ConfigurationManager.AppSettings["VersionProject"];
        public static readonly string TituloVentana = ConfigurationManager.AppSettings["TituloVentana"];

        public static string DireccionIP { get; set; }
        public static bool IsInOtherView { get; set; }
        #endregion
    }
}