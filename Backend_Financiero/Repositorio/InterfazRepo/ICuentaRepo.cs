using Modelos.ServicioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.InterfazRepo
{
    public interface ICuentaRepo
    {
        Task<IEnumerable<CuentaModelo>> ObtenerCuentas();
    }
}
