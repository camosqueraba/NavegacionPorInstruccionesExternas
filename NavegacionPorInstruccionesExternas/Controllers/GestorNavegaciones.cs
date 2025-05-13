using NavegacionPorInstruccionesExternas.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static NavegacionPorInstruccionesExternas.MainWindow;
using NavegacionPorInstruccionesExternas.Models;
using System.Globalization;

namespace NavegacionPorInstruccionesExternas.Controllers
{
    public class GestorNavegaciones
    {
        SeleniumCommands SeleniumCommandsInstance { get; set; }
        public GestorNavegaciones(SeleniumCommands seleniumCommands )
        {
              this.SeleniumCommandsInstance = seleniumCommands;  
        }

        public ResultadosInstruccionesNavegacion EjecutarInstruccionesNavegacion(InstruccionesNavegacion instruccionesNavegacion, ReportadorDeProgreso reportadorDeProgreso)
        {

            ResultadosInstruccionesNavegacion resultadosInstruccionesNavegacion = new ResultadosInstruccionesNavegacion();
            resultadosInstruccionesNavegacion.ResultadosAcciones = new List<ResultadoAccion>();
            ResultadoAccion resultadoAccion = null;

            if (instruccionesNavegacion != null && instruccionesNavegacion.AccionesNavegacion.Count > 0) {
                
                foreach (var accionNavegacion in instruccionesNavegacion.AccionesNavegacion)
                {
                    string tipoAccion = accionNavegacion.TipoAccion;
                    var localizadorBy = SeleniumCommandsInstance.EstableceLocalizador(accionNavegacion.TipoLocalizador, accionNavegacion.Localizador);

                    bool accionCompletada = false;
                    string valorRetornadoStr = null;
                    bool valorRetornadoBool = false;

                    reportadorDeProgreso(accionNavegacion.TipoAccion + " " + accionNavegacion.NombreLocalizador);
                    switch (tipoAccion)
                    {
                        case "visitar-url":
                            accionCompletada = SeleniumCommandsInstance.Visit(accionNavegacion.NombreLocalizador, new Uri(accionNavegacion.Localizador));
                            break;

                        case "click":
                            accionCompletada = SeleniumCommandsInstance.Click(accionNavegacion.NombreLocalizador, localizadorBy);
                            break;

                        case "digitar":
                            accionCompletada = SeleniumCommandsInstance.Type(accionNavegacion.NombreLocalizador, localizadorBy, accionNavegacion.TextoDigitado);
                            break;

                        case "obtener-texto":
                            valorRetornadoStr = SeleniumCommandsInstance.GetText(accionNavegacion.NombreLocalizador, localizadorBy);
                            break;

                        case "comparar-texto":
                            string textoRecuperado = SeleniumCommandsInstance.GetText(accionNavegacion.NombreLocalizador, localizadorBy);
                            resultadoAccion = CompararTexto(textoRecuperado, accionNavegacion.ContenidoEsperado);
                            break;

                        case "es-visible":
                            valorRetornadoBool = SeleniumCommandsInstance.IsDisplayed(accionNavegacion.NombreLocalizador, localizadorBy);
                            break;

                        case "verificar-url":
                            Uri url_original = new Uri(accionNavegacion.Localizador);
                            Uri url = SeleniumCommandsInstance.GetCurrentUrl();
                            if (url.AbsoluteUri == url_original.AbsoluteUri)
                                valorRetornadoBool = true;
                            break;

                        default:
                            break;
                    }

                    if (valorRetornadoBool || accionCompletada)
                        accionCompletada = true;
                    else
                        resultadosInstruccionesNavegacion.InstruccionError = accionNavegacion.TipoAccion + " : " + accionNavegacion.NombreLocalizador;

                    resultadoAccion = new ResultadoAccion(accionCompletada, valorRetornadoStr);
                    resultadosInstruccionesNavegacion.ResultadosAcciones.Add(resultadoAccion);
                }

            }
            
            return resultadosInstruccionesNavegacion;
        }     

        private ResultadoAccion CompararTexto(string textoObtenido, string textoEsperado)
        {
            try
            {
                ResultadoAccion resultadoAccion = new ResultadoAccion();

                if (textoObtenido != null && textoEsperado != null)
                {
                    resultadoAccion.Completado = true;

                    if (textoObtenido == textoEsperado)
                        resultadoAccion.ResultadoRetornado = textoObtenido;
                    else
                        resultadoAccion.ResultadoRetornado = "";
                }

                return resultadoAccion;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("GestorNavegaciones.CompararTexto == Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }
    }
}
