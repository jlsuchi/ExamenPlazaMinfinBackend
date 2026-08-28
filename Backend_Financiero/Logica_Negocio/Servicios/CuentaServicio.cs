using Logica_Negocio.Interfaz;
using Modelos.ServicioPago;
using Repositorio.InterfazRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica_Negocio.Servicios
{
    public class CuentaServicio : ICuenta
    {
        private readonly ICuentaRepo _cuentaRepositorio;

        public CuentaServicio(ICuentaRepo cuentaRepositorio)
        {
            _cuentaRepositorio = cuentaRepositorio;
        }

        public async Task<IEnumerable<CuentaModelo>> ObtenerCuentas()
        {
            try
            {
                return await _cuentaRepositorio.ObtenerCuentas();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<CuentaModelo>();
            }
        }
    }
}
