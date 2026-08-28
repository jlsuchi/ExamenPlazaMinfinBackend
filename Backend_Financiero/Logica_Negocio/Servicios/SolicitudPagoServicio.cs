using Logica_Negocio.Interfaz;
using Modelos;
using Modelos.ServicioPago;
using Repositorio.InterfazRepo;
using Repositorio.ServiciosRepo;

namespace Logica_Negocio.Servicios
{
    public class SolicitudPagoServicio : ISolicitudPago
    {
        private readonly ISolicitudPagoRepo _solicitudRepo;
        private readonly IRabbitMQ _rabbitMQ;

        public SolicitudPagoServicio(ISolicitudPagoRepo solicitudPagoRepo, IRabbitMQ rabbitMQ)
        {
            _solicitudRepo = solicitudPagoRepo;
            _rabbitMQ = rabbitMQ;
        }

        public async Task<IEnumerable<SolicitudPagoModelo>> ObtenerTodos()
        {
            try
            {
                return await _solicitudRepo.ObtenerTodos();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SolicitudPagoModelo>();
            }
        }

        public async Task<long> Insertar(SolicitudPagoModelo solicitud)
        {
            try
            {
                if (solicitud.Monto <= 0 || solicitud.IdCuenta <= 0) return 0;
                return await _solicitudRepo.Insertar(solicitud);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> Actualizar(SolicitudPagoModelo solicitud)
        {
            try
            {
                if (solicitud.IdSolicitudPago <= 0 || solicitud.Monto <= 0) return false;
                return await _solicitudRepo.Actualizar(solicitud);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(long idSolicitudPago)
        {
            try
            {
                if (idSolicitudPago <= 0) return false;
                return await _solicitudRepo.Eliminar(idSolicitudPago);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SolicitarPago(long idSolicitudPago)
        {


            // Por ejemplo cambiar estado a SOLICITADO.
            bool resultado = await _solicitudRepo.SolicitarPago(idSolicitudPago, "SOLICITADO");

            if (!resultado)
                return false;

            // Si la operación en BD fue correcta, encolamos el ID.
            await _rabbitMQ.EncolarSolicitudPago(idSolicitudPago);

            return true;
        }

        public async Task<IEnumerable<SolicitudPagoHistorialModelo>> ObtenerHistorial(long idSolicitudPago)
        {
            try
            {
                return await _solicitudRepo.ObtenerHistorial(idSolicitudPago);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SolicitudPagoHistorialModelo>();
            }
        }
    }
}