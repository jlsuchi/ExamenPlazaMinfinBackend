using Consumidor_SolicitudPago.Modelos;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Repositorio.InterfazRepo;
using System.Text;
using System.Text.Json;

namespace Consumidor_SolicitudPago
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _conexion;
        private IChannel? _canal;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"]!,
                Port = int.Parse(_configuration["RabbitMQ:Puerto"]!),
                UserName = _configuration["RabbitMQ:Usuario"]!,
                Password = _configuration["RabbitMQ:Password"]!
            };

            _conexion = await factory.CreateConnectionAsync();
            _canal = await _conexion.CreateChannelAsync();

            string exchange = "solicitud.exchange";
            string cola = "solicitud.pago";
            string colaRetry = "solicitud.pago.retry";
            string colaDLQ = "solicitud.pago.dlq";

            await _canal.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Topic, durable: true);

            var argumentosCola = new Dictionary<string, object?>
            {
                { "x-dead-letter-exchange", exchange },
                { "x-dead-letter-routing-key", "solicitud.pago.dlq.v1" }
            };

            await _canal.QueueDeclareAsync(queue: cola, durable: true, exclusive: false, autoDelete: false, arguments: argumentosCola);
            await _canal.QueueBindAsync(queue: cola, exchange: exchange, routingKey: "solicitud.pago.creada.v1");

            var argumentosRetry = new Dictionary<string, object?>
            {
                { "x-message-ttl", 30000 },
                { "x-dead-letter-exchange", exchange },
                { "x-dead-letter-routing-key", "solicitud.pago.creada.v1" }
            };

            await _canal.QueueDeclareAsync(queue: colaRetry, durable: true, exclusive: false, autoDelete: false, arguments: argumentosRetry);
            await _canal.QueueBindAsync(queue: colaRetry, exchange: exchange, routingKey: "solicitud.pago.retry.v1");

            await _canal.QueueDeclareAsync(queue: colaDLQ, durable: true, exclusive: false, autoDelete: false);
            await _canal.QueueBindAsync(queue: colaDLQ, exchange: exchange, routingKey: "solicitud.pago.dlq.v1");

            // Solo entrega 1 mensaje a la vez al consumidor
            await _canal.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumidor = new AsyncEventingBasicConsumer(_canal);
            long idSolicitudPago = 0;
            consumidor.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    string json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var mensaje = JsonSerializer.Deserialize<SolicitudPagoMensaje>(json);

                    if (mensaje == null)
                        throw new Exception("Mensaje inválido");

                    idSolicitudPago = mensaje.IdSolicitudPago;

                    Console.WriteLine($"Procesando ID_SOLICITUD_PAGO: {idSolicitudPago}");

                    using var scope = _scopeFactory.CreateScope();
                    var solicitudRepo = scope.ServiceProvider.GetRequiredService<ISolicitudPagoRepo>();

                    bool resultado = await solicitudRepo.SolicitarPago(idSolicitudPago, "PROCESANDO");

                    if (!resultado)
                        throw new Exception("No se pudo cambiar la solicitud a PROCESANDO");

                    // ERROR PROVOCADO PARA VER SI FUNCIONA  DLQ     D:
                    //throw new Exception("ERROR DE PRUEBA PARA ENVIAR A DLQ");


                    // Aquí iría la llamada real al banco, SOAP, API externa, etc.
                    bool resultadoPago = await solicitudRepo.SolicitarPago(idSolicitudPago, "PAGADO");

                    if (!resultadoPago)
                        throw new Exception("No se pudo cambiar la solicitud a PAGADO");


                    // Todo terminó correctamente
                    await _canal.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                    Console.WriteLine($"Solicitud {idSolicitudPago} procesada correctamente.");


                }
                catch (Exception ex)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var solicitudRepo = scope.ServiceProvider.GetRequiredService<ISolicitudPagoRepo>();
                    _logger.LogError(ex, "Error procesando solicitud");
                    bool resultadoPago2 = await solicitudRepo.SolicitarPago(idSolicitudPago, "ERROR");
                    // No vuelve inmediatamente a la misma cola.
                    // Se envía al mecanismo configurado de DLQ.
                    await _canal.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            await _canal.BasicConsumeAsync(queue: cola, autoAck: false, consumer: consumidor);

            Console.WriteLine($"Esperando mensajes en la cola {cola}...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}