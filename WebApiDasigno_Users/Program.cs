using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiDasigno_Users.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Creacion de variable para cadena de conexion
var connectionString = builder.Configuration.GetConnectionString("ConnectionDB");
//Registrar servicio
builder.Services.AddDbContext<AppDBcontext>(
    options => options.UseSqlServer(connectionString)
);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => { c.EnableAnnotations();});
var app = builder.Build();

//Crear base de datos al iniciar 
using (var Scope = app.Services.CreateScope())
{
    var dataContext = Scope.ServiceProvider.GetRequiredService<AppDBcontext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
