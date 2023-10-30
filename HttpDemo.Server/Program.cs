using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add API explorer services which will be used to generate Swagger/OpenAPI documents.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); // Use the first matching action
});

var app = builder.Build();

// Use routing middleware.
app.MapControllers();

// Use HTTPS redirection middleware.
app.UseHttpsRedirection();

// Use the Swagger UI middleware to display the Swagger/OpenAPI documents.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();