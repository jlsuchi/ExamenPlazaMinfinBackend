using Logica_Negocio.Interfaz;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Modelos.ServicioPago;
using Microsoft.AspNetCore.SignalR;
using Backend_Financiero.Hubs;

namespace Backend_Financiero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudPagoController : ControllerBase
    {
        private readonly ISolicitudPago _solicitudServicio;
        private readonly IHubContext<PagoHub> _hub;
        public SolicitudPagoController(ISolicitudPago solicitudServicio, IHubContext<PagoHub> hub)
        {
            _solicitudServicio = solicitudServicio;
            _hub = hub;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var solicitudes = await _solicitudServicio.ObtenerTodos();
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar las solicitudes." });//ocurrió una excepción dentro de tu backend.
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] SolicitudPagoModelo solicitud)
        {
            try
            {
                var id = await _solicitudServicio.Insertar(solicitud);
                if (id == 0) return BadRequest(new { mensaje = "No se pudo registrar la solicitud." });
                return Ok(new { mensaje = "Solicitud registrada correctamente.", idSolicitudPago = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al registrar la solicitud." }); //ocurrió una excepción dentro de tu backend.
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(long id, [FromBody] SolicitudPagoModelo solicitud)
        {
            try
            {
                solicitud.IdSolicitudPago = id;
                var resultado = await _solicitudServicio.Actualizar(solicitud);
                if (!resultado) return BadRequest(new { mensaje = "No se pudo actualizar la solicitud." });
                return Ok(new { mensaje = "Solicitud actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la solicitud." }); //ocurrió una excepción dentro de tu backend.
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(long id)
        {
            try
            {
                var resultado = await _solicitudServicio.Eliminar(id);
                if (!resultado) return NotFound(new { mensaje = "Solicitud no encontrada." });
                return Ok(new { mensaje = "Solicitud eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la solicitud." });//ocurrió una excepción dentro de tu backend.
            }
        }
        [HttpPost("solicitar/{idSolicitudPago}")]
        public async Task<IActionResult> SolicitarPago(long idSolicitudPago)
        {
            try
            {
                bool resultado = await _solicitudServicio.SolicitarPago(idSolicitudPago);
                if (!resultado)
                {
                    return BadRequest(new { mensaje = "No se pudo solicitar el pago" });
                }

                return Ok(new { mensaje = "Solicitud enviada correctamente", idSolicitudPago });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al solicitar el pago", error = ex.Message });
            }
        }

        [HttpGet("{idSolicitudPago}/historial")]
        public async Task<IActionResult> ObtenerHistorial(long idSolicitudPago)
        {
            try
            {
                var historial = await _solicitudServicio.ObtenerHistorial(idSolicitudPago);
                return Ok(historial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al consultar el historial de la solicitud." });
            }
        }

        [HttpPost("solicitud-estado/{idSolicitudPago}/{estado}")]
        public async Task<IActionResult> SolicitudEstado(long idSolicitudPago, string estado)
        {
            try
            {
                await _hub.Clients.All.SendAsync("SolicitudEstado", new
                {
                    idSolicitudPago,
                    estado
                });

                return Ok(new { mensaje = "Estado notificado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al notificar el estado", error = ex.Message });
            }
        }
    }
}