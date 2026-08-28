using Logica_Negocio.Interfaz;
using Microsoft.AspNetCore.Mvc;
using Repositorio.ad_usuarios;

namespace Backend_Financiero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IUsuario _usuarioServicio;
        public UsuarioController(IUsuario usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelo login)
        {
            if (login == null)
            {
                return BadRequest("Debe ingresar usuario y contraseña.");
            }

            var usuario = await _usuarioServicio.Login(login);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
            }

            return Ok(new
            {
                mensaje = "Login correcto.",
                usuario = new { usuario.IdUsuario, usuario.Usuario, usuario.Nombre, usuario.Estado }
            });
        }

        [HttpGet("entidades/{usuario}")]
        public async Task<IActionResult> ObtenerEntidades(string usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    return BadRequest(new { mensaje = "Debe indicar el usuario." });
                }

                var entidades = await _usuarioServicio.ObtenerEntidades(usuario);

                return Ok(entidades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al consultar las entidades." });
            }
        }
    }
}
