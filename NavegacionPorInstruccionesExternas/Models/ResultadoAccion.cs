using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas.Models
{
    public class ResultadoAccion
    {
        public bool Completado {  get; set; }
        public string ResultadoRetornado { get; set; }        

        public ResultadoAccion()
        {
                
        }

        public ResultadoAccion(bool completado, string resultado)
        {
            Completado = completado;
            ResultadoRetornado = resultado;
            
        }    
    }
}
