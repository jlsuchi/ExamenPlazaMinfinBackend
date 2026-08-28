using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.ServicioPago
{
    public class CuentaModelo
    {
        public long IdCuenta { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public string NombreCuenta { get; set; } = string.Empty;
    }
}
