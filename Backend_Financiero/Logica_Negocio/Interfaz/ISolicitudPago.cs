using Modelos.ServicioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_Negocio.Interfaz
{
    public interface ISolicitudPago
    {
        Task<IEnumerable<SolicitudPagoModelo>> ObtenerTodos();
        Task<long> Insertar(SolicitudPagoModelo solicitud);
        Task<bool> Actualizar(SolicitudPagoModelo solicitud);
        Task<bool> Eliminar(long idSolicitudPago);
        Task<bool> SolicitarPago(long idSolicitudPago);
        Task<IEnumerable<SolicitudPagoHistorialModelo>> ObtenerHistorial(long idSolicitudPago);
    }
}
