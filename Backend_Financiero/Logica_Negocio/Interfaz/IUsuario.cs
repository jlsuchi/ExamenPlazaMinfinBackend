using Modelos.Permisos;
using Repositorio.ad_usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_Negocio.Interfaz
{
    public interface IUsuario
    {
        Task<UsuarioModelo?> Login(LoginModelo login);
        Task<IEnumerable<EntidadUsuarioModelo>> ObtenerEntidades(string usuario);
    }
}
