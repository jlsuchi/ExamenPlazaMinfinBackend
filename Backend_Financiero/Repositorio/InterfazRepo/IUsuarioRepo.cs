using Modelos.Permisos;
using Repositorio.ad_usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.InterfazRepo
{
    public interface IUsuarioRepo
    {
        Task<UsuarioModelo?> ObtenerUsuario(string usuario);
        Task<IEnumerable<EntidadUsuarioModelo>> ObtenerEntidades(string usuario);
    }
}
