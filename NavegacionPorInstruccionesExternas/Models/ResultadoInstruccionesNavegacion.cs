using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas.Models
{
    public class ResultadosInstruccionesNavegacion 
    {
        public bool InstruccionesCompletadas { get; set; }
        public string InstruccionError { set; get; }
        public List<ResultadoAccion> ResultadosAcciones {  get; set; }
    }    
}