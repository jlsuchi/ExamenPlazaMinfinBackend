using Consumidor_SolicitudPago;
using Npgsql;
using Repositorio.InterfazRepo;
using Repositorio.ServiciosRepo;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return NpgsqlDataSource.Create(connectionString!);
});
// agrego el servicio para actualizar el estado
builder.Services.AddScoped<ISolicitudPagoRepo, SolicitudPagoRepo>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
