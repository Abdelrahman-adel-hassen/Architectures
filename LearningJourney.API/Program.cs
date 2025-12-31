var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(10),
                AutoReplenishment = true
            }));

    options.AddPolicy("strict", httpContext =>
      RateLimitPartition.GetFixedWindowLimiter(
          partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
          factory: _ => new FixedWindowRateLimiterOptions
          {
              PermitLimit = 1,
              Window = TimeSpan.FromSeconds(10),
              AutoReplenishment = true
          }));
    //global
    //options.AddFixedWindowLimiter("fixed", limiterOptions =>
    //{
    //    limiterOptions.PermitLimit = 1;
    //    limiterOptions.Window = TimeSpan.FromSeconds(10);
    //    limiterOptions.QueueLimit = 0;
    //});
});


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
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Add this to Program.cs for debugging

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseExceptionHandler(options => { });


app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.UseInfrastructure();

app.Run();
