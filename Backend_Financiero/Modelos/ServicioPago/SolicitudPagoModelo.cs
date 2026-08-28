using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.ServicioPago
{
    public class SolicitudPagoModelo
    {
        public long IdSolicitudPago { get; set; }
        public long Entidad { get; set; }
        public int UnidadEjecutora { get; set; }
        public int UnidadDesconcentrada { get; set; }
        public long IdUsuario { get; set; }
        public long IdCuenta { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
