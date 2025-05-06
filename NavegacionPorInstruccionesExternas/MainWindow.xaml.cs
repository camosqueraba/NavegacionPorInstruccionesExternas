using NavegacionPorInstruccionesExternas.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NavegacionPorInstruccionesExternas.DTOs;
using NavegacionPorInstruccionesExternas.Controller;
using static NavegacionPorInstruccionesExternas.MainWindow;

namespace NavegacionPorInstruccionesExternas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DateTime Tiempo_Inicial { get; set; }
        public static string IdUnique { get; set; }
        //public string Reporte { get; set; }

        public delegate void ReportadorDeProgreso(string reporte);

        private string reporte;

        public string Reporte
        {
            get { return reporte; }
            set { reporte = value; }
        }

        public MainWindow()
        {
            this.Title = GlobalVars.TituloVentana;

            string direccionIP = GetIPAddress();
            string nombreMaquina = GlobalVars.NombreMaquina = Environment.MachineName;
            string usuarioRed = GlobalVars.UsuarioRed = Environment.UserName;

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += Timer_Tick;
            LiveTime.Start();

            InitializeComponent();

            lbl_nombre_usuario.Content = usuarioRed + " ";
            lbl_informacion_maquina.Content = nombreMaquina + " " + direccionIP + " ";
            lbl_vesion_proyecto.Content = GlobalVars.VersionProject;

            
        }
        

        #region evitar cierre
        private void CerrarVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Desea cerrar el aplicativo?", GlobalVars.NameRPA, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Tiempo_Inicial = DateTime.Now;
                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> CerrarVentana" + "Se oprime el botón 'Yes' y se cierra la aplicación de manera manual");
                    //LOGRobotica.Controllers.LogWebServices.logsWS(Tiempo_Inicial, IdUnique, "Cerrar RPA", "Exitosa", "Cierre manual", "", "", "", "", "", "", "SAC_Sim_Login");
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> CerrarVentana" + "Se oprime el botón 'No' y se continúa en la Aplicación");
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow - CerrarVentana" + " -> " + "Exception: " + ex.Message);
            }
        }
        #endregion
        

        #region Obtener IP
        private static string GetIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> GetIPAddress" + " : " + "Exception" + ex.Message);
                return null;
            }
        }
        #endregion

        #region Reloj
        public void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lbl_fecha.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ";
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> timer_Tick" + " : " + " Exception: " + ex.Message);
            }
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportadorDeProgreso reportadorDeProgreso = ReportarProgreso;
            LogicaNavegacion.Instance.Navegar(reportadorDeProgreso);
        }

        public void ReportarProgreso(string reporte)
        {
            this.Reporte = reporte;
        }
    }
}
