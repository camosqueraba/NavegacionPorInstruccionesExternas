using NavegacionPorInstruccionesExternas.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace NavegacionPorInstruccionesExternas.Controllers
{
    public class GestorNavegaciones
    {
        SeleniumCommands SeleniumCommandsInstance { get; set; }
        public GestorNavegaciones(SeleniumCommands seleniumCommands )
        {
              this.SeleniumCommandsInstance = seleniumCommands;  
        }

        public bool EjecutarInstruccionesNavegacion(InstruccionesNavegacion instruccionesNavegacion)
        {
            bool result = false;
            if (instruccionesNavegacion != null && instruccionesNavegacion.AccionesNavegacion.Count > 0) {
                foreach (var accionNavegacion in instruccionesNavegacion.AccionesNavegacion)
                {
                    string tipoAccion = accionNavegacion.TipoAccion;

                    switch (tipoAccion)
                    {
                        case "visit":
                            SeleniumCommandsInstance.Visit(accionNavegacion.NombreLocalizador, new Uri(accionNavegacion.Localizador));
                            break;

                        case "type":
                            string tipoSelector = accionNavegacion.TipoLocalizador;
                            string localizador = accionNavegacion.Localizador;
                            By localizadorBy = EstableceLocalizador(tipoSelector, localizador); 
                            SeleniumCommandsInstance.Type(accionNavegacion.NombreLocalizador, localizadorBy, accionNavegacion.TextoDigitado);
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

                    default:
                        break;
                }
            }
            return result;
        }
    }
}
