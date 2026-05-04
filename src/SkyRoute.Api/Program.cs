var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// TODO: Add FluentValidation
// builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// TODO: Add Application layer services
// builder.Services.AddApplicationServices();

// TODO: Add Infrastructure layer services (EF Core, providers, pricing strategies)
// builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

// TODO: Add custom middleware here
// app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
