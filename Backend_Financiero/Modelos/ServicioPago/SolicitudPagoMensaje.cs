using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.ServicioPago
{
    public class SolicitudPagoMensaje
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Version { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public DateTime Fecha { get; set; }
        public long IdSolicitudPago { get; set; }
    }
}
