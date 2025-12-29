var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var applicationAssembly = typeof(ApplocationExtenstion).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(applicationAssembly);
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

var app = builder.Build();

// Add this to Program.cs for debugging

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseInfrastructure();

app.Run();
