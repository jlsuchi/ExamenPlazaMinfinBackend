using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Permisos
{
    public class EntidadUsuarioModelo
    {
        public long Entidad { get; set; }

        public int UnidadEjecutora { get; set; }

        public int UnidadDesconcentrada { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }
}
