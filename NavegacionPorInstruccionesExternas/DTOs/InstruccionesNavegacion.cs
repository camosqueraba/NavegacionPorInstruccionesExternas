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
        public List<AccionNavegacion> AccionesNavegacion { get; set; }

        /*
        public InstruccionesNavegacion()
        {
            this.MyProperty = 1;
            this.MyProperty2 = 1;
            this.MyProperty3 = 1;
            this.AccionesNavegacion = new List<AccionNavegacion>{
            new AccionNavegacion(),
            new AccionNavegacion(),
            new AccionNavegacion()
            };
        }
        */
    }
}
