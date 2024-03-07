using AlimentandoEsperanzas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AlimentandoesperanzasContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql")));

// Configurar servicios de sesi�n
builder.Services.AddSession();

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // El valor HSTS predeterminado es de 30 d�as. Puedes cambiar esto para escenarios de producci�n, consulta https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Agregar middleware de sesi�n antes del middleware de autorizaci�n
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
