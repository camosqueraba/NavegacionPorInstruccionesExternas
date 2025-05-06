using NavegacionPorInstruccionesExternas.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using static NavegacionPorInstruccionesExternas.MainWindow;

namespace NavegacionPorInstruccionesExternas.Controllers
{
    public class GestorNavegaciones
    {
        SeleniumCommands SeleniumCommandsInstance { get; set; }
        public GestorNavegaciones(SeleniumCommands seleniumCommands )
        {
              this.SeleniumCommandsInstance = seleniumCommands;  
        }

        public bool EjecutarInstruccionesNavegacion(InstruccionesNavegacion instruccionesNavegacion, ReportadorDeProgreso reportadorDeProgreso)
        {
            bool result = false;
            if (instruccionesNavegacion != null && instruccionesNavegacion.AccionesNavegacion.Count > 0) {
                foreach (var accionNavegacion in instruccionesNavegacion.AccionesNavegacion)
                {
                    string tipoAccion = accionNavegacion.TipoAccion;
                    By localizadorBy = EstableceLocalizador(accionNavegacion.TipoLocalizador, accionNavegacion.Localizador);

                    reportadorDeProgreso(accionNavegacion.TipoAccion + " " + accionNavegacion.NombreLocalizador);
                    switch (tipoAccion)
                    {
                        case "visit":
                            result = SeleniumCommandsInstance.Visit(accionNavegacion.NombreLocalizador, new Uri(accionNavegacion.Localizador));
                            break;

                        case "click":
                            SeleniumCommandsInstance.Click(accionNavegacion.NombreLocalizador, localizadorBy);
                            break;

                        case "type": 
                            SeleniumCommandsInstance.Type(accionNavegacion.NombreLocalizador, localizadorBy, accionNavegacion.TextoDigitado);
                            break;

                        case "is-visible":
                            SeleniumCommandsInstance.IsDisplayed(accionNavegacion.NombreLocalizador, localizadorBy);
                            break;

                        case "verificar-url":
                            Uri url_original = new Uri(accionNavegacion.Localizador);
                            Uri url = SeleniumCommandsInstance.GetCurrentUrl();
                            if (url.AbsoluteUri == url_original.AbsoluteUri)
                                result = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            
            return result;
        }

        private By EstableceLocalizador(string tipoLocalizador, string localizador)
        {
            By result = null;
            if (!string.IsNullOrEmpty(tipoLocalizador))
            {
                switch (tipoLocalizador)
                {
                    case "id":
                        result = By.Id(localizador);
                        break;

                    case "name":
                        result = By.Name(localizador);
                        break;

                    case "css":
                        result = By.CssSelector(localizador);
                        break;

                    default:
                        break;
                }
            }
            return result;
        }


    }
}
