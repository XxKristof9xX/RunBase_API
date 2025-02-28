using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using RunBase_API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();


builder.Services.AddControllers();
builder.Services.AddDbContext<RunBaseDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("https://www.runbase.hu");

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
