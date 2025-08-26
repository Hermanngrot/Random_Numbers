using Scalar.AspNetCore; // <- para /scalar

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();

// OpenAPI nativo de ASP.NET Core (.NET 8)
builder.Services.AddOpenApi();

// (Opcional) Si quieres también Swagger clásico, descomenta estas dos líneas
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    // Expone el documento OpenAPI (por defecto: /openapi/v1.json)
    // Puedes fijar una ruta explícita si quieres:
    app.MapOpenApi("/openapi/v1.json");

    // UI moderna Scalar en /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.Title = "Random Numbers API";
        // options.Theme = ScalarTheme.Mars; // opcional: cambia el tema
        // options.DefaultHttpClient = new ScalarReferenceHttpClient("https://localhost:5109"); // opcional
    });

    // (Opcional) Swagger UI clásico: requiere Swashbuckle.AspNetCore
    // app.UseSwagger();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "Random Numbers API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();