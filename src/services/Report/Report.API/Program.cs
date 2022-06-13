using Core;
using Core.Models;
using Core.Services;
using Core.Services.Interfaces;
using DataAccess.Concretes;
using DataAccess.Interfaces;
using DataAccess.Manager;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Report.Service;
using Report.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Service extensions
builder.Services.AddSingleton<IOptions<ServiceOptions>, ServiceSettings>();
builder.Services.AddTransient<IDBManager, DBManager>();
builder.Services.AddTransient<IDataAccess, PostgresDataAccess>();
builder.Services.AddTransient<IReportService, ReportService>();
builder.Services.AddTransient<IApiClientService, ApiClientService>();

builder.Services.AddHostedService<ReportQueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Upload")),
    RequestPath = "/upload"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
