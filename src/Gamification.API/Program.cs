using Gamification.Infrastructure.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Use InMemory for now
builder.Services.AddInfrastructure(
    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
    useInMemory: true
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Landing page
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
""", "text/html"));


app.MapControllers();

app.Run();
