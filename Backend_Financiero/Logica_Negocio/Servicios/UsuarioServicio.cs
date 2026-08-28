using Logica_Negocio.Interfaz;
using Modelos.Permisos;
using Repositorio.ad_usuarios;
using Repositorio.InterfazRepo;
using Repositorio.ServiciosRepo;
using System.Text;

namespace Logica_Negocio.Servicios
{
    public class UsuarioServicio : IUsuario
    {
        private readonly IUsuarioRepo _usuarioRepositorio;

        public UsuarioServicio(IUsuarioRepo usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<UsuarioModelo?> Login(LoginModelo login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login.Usuario))
                    return null;

                if (string.IsNullOrWhiteSpace(login.Password))
                    return null;

                var usuario = await _usuarioRepositorio
                    .ObtenerUsuario(login.Usuario);

                if (usuario == null)
                    return null;

                if (!usuario.Estado)
                    return null;

                // Convertir el password ingresado a Base64
                string passwordBase64 = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(login.Password)
                );

                // Comparar con lo almacenado en PostgreSQL
                if (usuario.PasswordHash != passwordBase64)
                    return null;

                return usuario;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public async Task<IEnumerable<EntidadUsuarioModelo>> ObtenerEntidades(
           string usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario))
                {
                    return Enumerable.Empty<EntidadUsuarioModelo>();
                }

                return await _usuarioRepositorio.ObtenerEntidades(usuario);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<EntidadUsuarioModelo>();
            }
        }



    }
}