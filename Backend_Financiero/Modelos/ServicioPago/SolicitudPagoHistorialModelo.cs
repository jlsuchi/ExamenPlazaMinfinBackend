using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.ServicioPago
{
    public class SolicitudPagoHistorialModelo
    {
        public long IdSolicitudPago { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int NumeroIntento { get; set; }
        public DateTime Fecha { get; set; }
    }
}
