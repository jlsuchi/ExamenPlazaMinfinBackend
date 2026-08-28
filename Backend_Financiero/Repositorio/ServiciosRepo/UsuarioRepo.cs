using Dapper;
using Modelos.Permisos;
using MongoDB.Driver.Core.Configuration;
using Npgsql;
using Repositorio.ad_usuarios;
using Repositorio.InterfazRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.ServiciosRepo
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly NpgsqlDataSource _dataSource;

        public UsuarioRepo(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<UsuarioModelo?> ObtenerUsuario(string usuario)
        {
            try
            {
                const string sql = @"
                            SELECT
                                id_usuario AS IdUsuario,
                                usuario AS Usuario,
                                nombre AS Nombre,
                                correo AS Correo,
                                password_hash AS PasswordHash,
                                estado AS Estado
                            FROM usuario
                            WHERE usuario = @Usuario";

                await using var conexion =
                    await _dataSource.OpenConnectionAsync();

                return await conexion.QueryFirstOrDefaultAsync<UsuarioModelo>(sql, new { Usuario = usuario }
                );
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
                const string sql = @"
                SELECT DISTINCT
                       e.entidad AS Entidad,
                       e.unidad_ejecutora AS UnidadEjecutora,
                       e.unidad_desconcentrada AS UnidadDesconcentrada,
                       e.nombre AS Nombre
                FROM usuario u
                INNER JOIN usuario_acceso ua
                        ON ua.id_usuario = u.id_usuario
                INNER JOIN entidad e
                        ON e.entidad = ua.entidad
                       AND e.unidad_ejecutora = ua.unidad_ejecutora
                       AND e.unidad_desconcentrada = ua.unidad_desconcentrada
                WHERE u.usuario = @Usuario
                  AND u.estado = TRUE
                  AND e.estado = TRUE
                ORDER BY e.entidad,
                         e.unidad_ejecutora,
                         e.unidad_desconcentrada;
            ";

                await using var conexion =
             await _dataSource.OpenConnectionAsync();

                var resultado =
            await conexion.QueryAsync<EntidadUsuarioModelo>(sql, new { Usuario = usuario });


                return resultado;

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<EntidadUsuarioModelo>();
            }

        }
    }
}
