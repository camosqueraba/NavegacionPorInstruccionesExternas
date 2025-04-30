using NavegacionPorInstruccionesExternas.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas
{
    public class LogicaNavegacion
    {
        SeleniumCommands SeleniumCommands { get; set; }
        //DownloadsPage DownloadsPage { get; set; }

        private static LogicaNavegacion instance = null;

        public static LogicaNavegacion Instance
        {
            get
            {
                if (instance == null)
                    instance = new LogicaNavegacion();
                return instance;
            }
        }


        #region Constructor
        protected LogicaNavegacion()
        {
            try
            {
                this.SeleniumCommands = new SeleniumCommands();
                //GeraPage = new GERAPage(SeleniumCommands);
                //SharepointPage = new SharepointPage(SeleniumCommands);
                //DownloadsPage = new DownloadsPage(SeleniumCommands);
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("RobotBarridos - Constructor" + " : " + "Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
            }
        }
        #endregion

        public void Navegar()
        {
            bool canContinue = false;
            canContinue = SeleniumCommands.Visit("Pagina login Gera", new Uri("https://www.google.com/"));
        }
    }
}
