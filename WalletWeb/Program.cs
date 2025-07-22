using Application.Services;
using Application.Interfaces;
using Domain.Model.Interfaces;
using Infra.DataAccess.Data;
using Infra.DataAccess.Repository;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<Domain.Model.Interfaces.IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IContabilidadRepository, ContabilidadRepository>();
builder.Services.AddScoped<IContabilidadService, ContabilidadService>();


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
