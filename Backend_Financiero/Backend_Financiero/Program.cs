
using Logica_Negocio.Interfaz;
using Logica_Negocio.Servicios;
using Npgsql;
using Repositorio.InterfazRepo;
using Repositorio.ServiciosRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSignalR(); // se agrega SignalR

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión DefaultConnection.");

var dataSource = NpgsqlDataSource.Create(connectionString);
//Postgresql  
builder.Services.AddSingleton(dataSource);

// conectamos con MongoDB

string mongoConnectionString =
    builder.Configuration.GetConnectionString("MongoDB")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión MongoDB.");

// Definicion de los servicios
builder.Services.AddScoped<IUsuario, UsuarioServicio>();
builder.Services.AddScoped<IUsuarioRepo, UsuarioRepo>();
builder.Services.AddScoped<ICuenta, CuentaServicio>();
builder.Services.AddScoped<ICuentaRepo, CuentaRepo>();
builder.Services.AddScoped<ISolicitudPago, SolicitudPagoServicio>();
builder.Services.AddScoped<ISolicitudPagoRepo, SolicitudPagoRepo>();
builder.Services.AddScoped<IRabbitMQ, RabbitMQServicio>();

// permitir Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// permitir angular
app.UseCors("PermitirAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Backend Banco API v1"
        );

        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("Angular");
app.UseAuthorization();

app.MapControllers();
app.Run();
