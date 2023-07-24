//dotnet add package Microsoft.AspNetCore.DataProtection
//dotnet add package RepoDb.PostgreSql
//dotnet add package Npgsql

using Microsoft.AspNetCore.DataProtection;
using RepoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("DistributedDpApiDemo");

GlobalConfiguration
	.Setup()
	.UsePostgreSql();

var app = builder.Build();

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
