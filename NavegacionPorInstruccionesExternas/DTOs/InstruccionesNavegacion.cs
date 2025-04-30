using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavegacionPorInstruccionesExternas.DTOs
{
    public class InstruccionesNavegacion
    {
        public int MyProperty { get; set; }
        public int MyProperty2 { get; set; }
        public int MyProperty3 { get; set; }
        public List<AccionNavegacion> AccionNavegacion { get; set; }
    }
}
