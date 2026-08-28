using Modelos.ServicioPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_Negocio.Interfaz
{
    public interface ICuenta
    {
        Task<IEnumerable<CuentaModelo>> ObtenerCuentas();
    }
}
