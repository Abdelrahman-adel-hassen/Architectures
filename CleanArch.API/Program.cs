using CleanArch.Infrastructure;
using CleanArch.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
//var applicationAssembly = typeof(ApplicationBuilderExtensions).Assembly;

//builder.Services.AddMediatR(config =>
//{
//    config.RegisterServicesFromAssemblies(applicationAssembly);
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
