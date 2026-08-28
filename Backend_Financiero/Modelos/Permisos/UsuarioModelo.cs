using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.ad_usuarios
{
    public class UsuarioModelo
    {
        public long IdUsuario { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string? PasswordHash { get; set; }
        public bool Estado { get; set; }
    }
}
