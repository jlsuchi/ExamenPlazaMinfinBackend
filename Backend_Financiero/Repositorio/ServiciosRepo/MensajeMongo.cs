using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.ServiciosRepo
{
    public class MensajeMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Version { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public DateTime Fecha { get; set; }
        public long IdSolicitudPago { get; set; }

        public string EstadoProceso { get; set; } = "PENDIENTE";
        public int Intento { get; set; } = 1;
    }
}
