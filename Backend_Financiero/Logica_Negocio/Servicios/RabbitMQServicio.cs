using Logica_Negocio.Interfaz;
using Microsoft.Extensions.Configuration;
using Modelos.ServicioPago;
using MongoDB.Driver;
using RabbitMQ.Client;
using Repositorio.ServiciosRepo;
using System.Text;
using System.Text.Json;

namespace Logica_Negocio.Servicios
{
    public class RabbitMQServicio : IRabbitMQ
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<MensajeMongo> _mensajeMongo;
        public RabbitMQServicio(IConfiguration configuration)
        {
            _configuration = configuration;

            string mongoConnection = configuration.GetConnectionString("MongoDB")
               ?? throw new InvalidOperationException("No se encontró MongoDB.");

            var mongoClient = new MongoClient(mongoConnection);
            var mongoDatabase = mongoClient.GetDatabase("PAGOS");

            _mensajeMongo = mongoDatabase.GetCollection<MensajeMongo>("PAGOS");
        }

        public async Task EncolarSolicitudPago(long idSolicitudPago)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? throw new Exception("RabbitMQ:Host no configurado"),
                Port = int.Parse(_configuration["RabbitMQ:Puerto"] ?? throw new Exception("RabbitMQ:Puerto no configurado")),
                UserName = _configuration["RabbitMQ:Usuario"] ?? throw new Exception("RabbitMQ:Usuario no configurado"),
                Password = _configuration["RabbitMQ:Password"] ?? throw new Exception("RabbitMQ:Password no configurado")
            };

            await using var conexion = await factory.CreateConnectionAsync();
            await using var canal = await conexion.CreateChannelAsync();

            string exchange = "solicitud.exchange";
            string cola = "solicitud.pago";
            string colaRetry = "solicitud.pago.retry";
            string colaDLQ = "solicitud.pago.dlq";
            string colaParking = "solicitud.pago.parking";
            string routingKey = "solicitud.pago.creada.v1";

            await canal.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true);

            var argumentosCola = new Dictionary<string, object?>
            {
                { "x-dead-letter-exchange", exchange },
                { "x-dead-letter-routing-key", "solicitud.pago.dlq.v1" }
            };

            await canal.QueueDeclareAsync(queue: cola, durable: true, exclusive: false, autoDelete: false, arguments: argumentosCola);
            await canal.QueueBindAsync(queue: cola, exchange: exchange, routingKey: routingKey);

            var argumentosRetry = new Dictionary<string, object?>
            {
                { "x-message-ttl", 30000 },
                { "x-dead-letter-exchange", exchange },
                { "x-dead-letter-routing-key", routingKey }
            };

            await canal.QueueDeclareAsync(queue: colaRetry, durable: true, exclusive: false, autoDelete: false, arguments: argumentosRetry);
            await canal.QueueBindAsync(queue: colaRetry, exchange: exchange, routingKey: "solicitud.pago.retry.v1");

            await canal.QueueDeclareAsync(queue: colaDLQ, durable: true, exclusive: false, autoDelete: false);
            await canal.QueueBindAsync(queue: colaDLQ, exchange: exchange, routingKey: "solicitud.pago.dlq.v1");

            await canal.QueueDeclareAsync(queue: colaParking, durable: true, exclusive: false, autoDelete: false);
            await canal.QueueBindAsync(queue: colaParking, exchange: exchange, routingKey: "solicitud.pago.parking.v1");

            var mensaje = new SolicitudPagoMensaje
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "SOLICITUD_PAGO_CREADA",
                Version = 1,
                CorrelationId = Guid.NewGuid().ToString(),
                Sequence = 1,
                Fecha = DateTime.UtcNow,
                IdSolicitudPago = idSolicitudPago
            };

            string json = JsonSerializer.Serialize(mensaje);
            byte[] body = Encoding.UTF8.GetBytes(json);

            var propiedades = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                MessageId = mensaje.EventId,
                CorrelationId = mensaje.CorrelationId
            };

            await canal.BasicPublishAsync(exchange: exchange, routingKey: routingKey, mandatory: true, basicProperties: propiedades, body: body);

            // REGISTRO EN MONGODB
            var mensajeMongo = new MensajeMongo
            {
                EventId = mensaje.EventId,
                EventType = mensaje.EventType,
                Version = mensaje.Version,
                CorrelationId = mensaje.CorrelationId,
                Sequence = mensaje.Sequence,
                Fecha = mensaje.Fecha,
                IdSolicitudPago = mensaje.IdSolicitudPago,
                EstadoProceso = "ENCOLADO",
                Intento = 1
            };

            await _mensajeMongo.InsertOneAsync(mensajeMongo);
        }
    }
}