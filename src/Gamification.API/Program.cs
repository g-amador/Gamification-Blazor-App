using Gamification.Api.Middleware;
using Gamification.Infrastructure.Data;
using Gamification.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    // Scalar renders grouped headings in tag-declaration order;
    // sort the tags alphabetically so groups appear in a stable order.
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Tags = new SortedSet<OpenApiTag>(
            document.Tags ?? new HashSet<OpenApiTag>(),
            Comparer<OpenApiTag>.Create((a, b) =>
                string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase)));
        return Task.CompletedTask;
    });
});

// Database provider selection: LocalDB (SQL Server) by default,
// EF InMemory when the Database:UseInMemory flag is set to true
var useInMemory = builder.Configuration.GetValue("Database:UseInMemory", false);
builder.Services.AddInfrastructure(
    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
    useInMemory: useInMemory
);

var app = builder.Build();

// Apply EF Core migrations when running against a relational database
if (!useInMemory)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<GamificationDbContext>();
        db.Database.Migrate();
    }
}

// Translate application exceptions (UnauthorizedException → 401 ProblemDetails)
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Landing page (HTML only — not part of the API reference)
app.MapGet("/", () => Results.Text("""
    <html>
        <head>
            <title>Gamification API</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                h1 { color: #4a4a4a; }
                a { color: #0078d4; text-decoration: none; }
                a:hover { text-decoration: underline; }
                .card {
                    padding: 20px;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    max-width: 500px;
                }
            </style>
        </head>
        <body>
            <div class="card">
                <h1>Gamification API</h1>
                <p>Status: Running</p>
                <p>Environment: Development</p>
                <p><a href="/scalar">Scalar</a></p>
                <p><a href="/openapi/v1.json">OpenAPI JSON</a></p>
            </div>
        </body>
    </html>
""", "text/html")).ExcludeFromDescription();


app.MapControllers();

app.Run();
