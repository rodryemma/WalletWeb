using Application.Interfaces;
using Application.Services;
using Domain.Model.Interfaces;
using Infra.DataAccess.Data;
using Infra.DataAccess.Repositories;
using Infra.DataAccess.Repository;
using Infra.ExternalServices.Api;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<Domain.Model.Interfaces.IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IContabilidadRepository, ContabilidadRepository>();
builder.Services.AddScoped<IContabilidadService, ContabilidadService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IDivisaRepository, DivisaRepository>();
builder.Services.AddScoped<IDivisaService, DivisaService>();
builder.Services.AddScoped<ICuentaWalletRepository, CuentaWalletRepository>();
builder.Services.AddScoped<ICuentaWalletService, CuentaWalletService>();
builder.Services.AddScoped<ITransferenciaRepository, TransferenciaRepository>();
builder.Services.AddScoped<ITransferenciaService, TransferenciaService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddSingleton<IDolarService, DolarService>();
builder.Services.AddHttpClient<IDolarArgentinaApi, DolarArgentinaApi>();
builder.Services.AddHttpClient<IAmbitoApi, AmbitoApi>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
