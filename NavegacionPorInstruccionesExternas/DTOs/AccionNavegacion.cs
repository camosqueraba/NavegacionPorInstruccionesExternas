using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas.DTOs
{
    public class AccionNavegacion
    {
        public string TipoAccion {  get; set; }
        public string TipoLocalizador { get; set; }
        public string Localizador { get; set; }
        public string NombreLocalizador { get; set; }
        public string TextoDigitado { get; set; }
        public string ContenidoRetornado {  get; set; }
    }
}
