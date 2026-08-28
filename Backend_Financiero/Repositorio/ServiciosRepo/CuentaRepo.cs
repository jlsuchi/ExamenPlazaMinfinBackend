using Dapper;
using Modelos.ServicioPago;
using Npgsql;
using Repositorio.InterfazRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.ServiciosRepo
{
    public class CuentaRepo : ICuentaRepo
    {
        private readonly NpgsqlDataSource _dataSource;

        public CuentaRepo(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<IEnumerable<CuentaModelo>> ObtenerCuentas()
        {
            try
            {
                const string sql = @"SELECT id_cuenta AS IdCuenta, numero_cuenta AS NumeroCuenta, nombre_cuenta AS NombreCuenta FROM cuenta_bancaria ORDER BY numero_cuenta";

                await using var conexion = await _dataSource.OpenConnectionAsync();

                var resultado = await conexion.QueryAsync<CuentaModelo>(sql);

                return resultado;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<CuentaModelo>();
            }
        }
    }
}
