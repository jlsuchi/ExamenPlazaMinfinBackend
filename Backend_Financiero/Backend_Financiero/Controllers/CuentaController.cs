using Logica_Negocio.Interfaz;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Financiero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuenta _cuentaServicio;

        public CuentaController(ICuenta cuentaServicio)
        {
            _cuentaServicio = cuentaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCuentas()
        {
            try
            {
                var cuentas = await _cuentaServicio.ObtenerCuentas();

                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al consultar las cuentas." });
            }
        }
    }
}
