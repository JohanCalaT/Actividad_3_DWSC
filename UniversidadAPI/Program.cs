using Microsoft.EntityFrameworkCore;
using UniversidadAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddDbContext<UniversidadContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectar dependencia
builder.Services.AddScoped<UniversidadAPI.Services.IUserService, UniversidadAPI.Services.UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Importante para CSS
app.MapControllers();

app.Run();
