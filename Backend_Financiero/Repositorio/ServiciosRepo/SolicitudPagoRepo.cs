using Dapper;
using Modelos;
using Modelos.ServicioPago;
using Npgsql;
using Repositorio.InterfazRepo;

namespace Repositorio.ServiciosRepo
{
    public class SolicitudPagoRepo : ISolicitudPagoRepo
    {
        private readonly NpgsqlDataSource _dataSource;

        public SolicitudPagoRepo(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<IEnumerable<SolicitudPagoModelo>> ObtenerTodos()
        {
            try
            {
                const string sql = @"SELECT id_solicitud_pago AS IdSolicitudPago, entidad AS Entidad, unidad_ejecutora AS UnidadEjecutora, unidad_desconcentrada AS UnidadDesconcentrada, id_usuario AS IdUsuario, id_cuenta AS IdCuenta, descripcion AS Descripcion, monto AS Monto, estado AS Estado, fecha_creacion AS FechaCreacion, fecha_actualizacion AS FechaActualizacion FROM solicitud_pago ORDER BY id_solicitud_pago DESC";
                await using var conexion = await _dataSource.OpenConnectionAsync();
                return await conexion.QueryAsync<SolicitudPagoModelo>(sql);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SolicitudPagoModelo>();
            }
        }

        public async Task<long> Insertar(SolicitudPagoModelo solicitud)
        {
            try
            {
                const string sql = @"INSERT INTO solicitud_pago (entidad, unidad_ejecutora, unidad_desconcentrada, id_usuario, id_cuenta, descripcion, monto) VALUES (@Entidad, @UnidadEjecutora, @UnidadDesconcentrada, @IdUsuario, @IdCuenta, @Descripcion, @Monto) RETURNING id_solicitud_pago";
                await using var conexion = await _dataSource.OpenConnectionAsync();
                return await conexion.ExecuteScalarAsync<long>(sql, solicitud);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> Actualizar(SolicitudPagoModelo solicitud)
        {
            try
            {
                const string sql = @"UPDATE solicitud_pago SET id_cuenta = @IdCuenta, descripcion = @Descripcion, monto = @Monto, fecha_actualizacion = CURRENT_TIMESTAMP WHERE id_solicitud_pago = @IdSolicitudPago";
                await using var conexion = await _dataSource.OpenConnectionAsync();
                var filas = await conexion.ExecuteAsync(sql, solicitud);
                return filas > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(long idSolicitudPago)
        {
            try
            {
                const string sql = @"DELETE FROM solicitud_pago WHERE id_solicitud_pago = @IdSolicitudPago";
                await using var conexion = await _dataSource.OpenConnectionAsync();
                var filas = await conexion.ExecuteAsync(sql, new { IdSolicitudPago = idSolicitudPago });
                return filas > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SolicitarPago(long idSolicitudPago, string Varestado)
        {
            using var conexion = _dataSource.CreateConnection();
            await conexion.OpenAsync();
            using var transaccion = await conexion.BeginTransactionAsync();

            try
            {
                string sqlSolicitud = @"UPDATE solicitud_pago SET estado = @Estado, fecha_actualizacion = CURRENT_TIMESTAMP WHERE id_solicitud_pago = @IdSolicitudPago";
                int resultado = await conexion.ExecuteAsync(sqlSolicitud, new { IdSolicitudPago = idSolicitudPago, Estado = Varestado }, transaccion);

                if (resultado == 0)
                {
                    await transaccion.RollbackAsync();
                    return false;
                }

                string sqlHistorial = @"INSERT INTO solicitud_pago_historial (id_solicitud_pago, numero_intento, estado, detalle) VALUES (@IdSolicitudPago, 1, @Estado, 'Solicitud para procesamiento')";
                await conexion.ExecuteAsync(sqlHistorial, new { IdSolicitudPago = idSolicitudPago, Estado = Varestado }, transaccion);

                await transaccion.CommitAsync();


                return true;
            }
            catch
            {
                await transaccion.RollbackAsync();

                string sqlSolicitud = @"UPDATE solicitud_pago SET estado = 'ERROR', fecha_actualizacion = CURRENT_TIMESTAMP WHERE id_solicitud_pago = @IdSolicitudPago";
                await conexion.ExecuteAsync(sqlSolicitud, new { IdSolicitudPago = idSolicitudPago });

                string sqlHistorial = @"INSERT INTO solicitud_pago_historial (id_solicitud_pago, numero_intento, estado, detalle) VALUES (@IdSolicitudPago, 1, 'ERROR', 'ERROR: se realiza la reversión')";
                await conexion.ExecuteAsync(sqlHistorial, new { IdSolicitudPago = idSolicitudPago });

                throw;
            }
        }


        public async Task<IEnumerable<SolicitudPagoHistorialModelo>> ObtenerHistorial(long idSolicitudPago)
        {
            try
            {
                const string sql = @"
                                SELECT
                                    ID_SOLICITUD_PAGO AS IdSolicitudPago,
                                    ESTADO AS Estado,
                                    NUMERO_INTENTO AS NumeroIntento,
                                    FECHA AS Fecha
                                FROM SOLICITUD_PAGO_HISTORIAL
                                WHERE ID_SOLICITUD_PAGO = @IdSolicitudPago
                                ORDER BY FECHA DESC";

                using var conexion = _dataSource.CreateConnection();
                return await conexion.QueryAsync<SolicitudPagoHistorialModelo>(sql, new { IdSolicitudPago = idSolicitudPago });
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<SolicitudPagoHistorialModelo>();
            }


        }
    }
}