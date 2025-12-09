using Crates.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.AI;



var builder = WebApplication.CreateBuilder(args);

// 1. Add Aspire Service Defaults (Metrics, Health Checks, Logging)
builder.AddServiceDefaults();

// 2. Add Services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This prevents the "Album -> Artist -> Album" infinite loop
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOllamaEmbeddingGenerator( // Note: New method name
    modelId: "all-minilm",
    endpoint: new Uri("http://localhost:11434")
);

// 3. REGISTER THE DATABASE CONTEXT
// Aspire injects the connection string named "DefaultConnection" automatically.
builder.Services.AddDbContext<CratesContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b =>
        {
            b.MigrationsAssembly("Crates.Data");
            b.UseVector();
        }
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<CratesContext>();

        var generator = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        await DbInitializer.InitializeAsync(context, generator);
    }
}

// 4. Configure the HTTP request pipeline
app.MapDefaultEndpoints(); // Required for Aspire health checks

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();