using Crates.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Pgvector.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel;

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

builder.Services.AddOpenAITextEmbeddingGeneration(
    modelId: "text-embedding-3-small",
    apiKey: builder.Configuration["OpenAI:ApiKey"]!
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

        var embeddingService = scope.ServiceProvider.GetRequiredService<ITextEmbeddingGenerationService>();

        await DbInitializer.InitializeAsync(context, embeddingService);
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