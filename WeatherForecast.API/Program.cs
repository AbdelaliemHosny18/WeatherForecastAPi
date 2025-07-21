using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WeatherForecast.API.Extensions;
using WeatherForecast.API.Middlewares;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Application.Models;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Infrastructure.Repositories;
using WeatherForecast.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddServices(builder); 


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();



app.Run();


