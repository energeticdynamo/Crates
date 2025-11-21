using Crates.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Aspire Service Defaults (Metrics, Health Checks, Logging)
builder.AddServiceDefaults();

// 2. Add Services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. REGISTER THE DATABASE CONTEXT
// Aspire injects the connection string named "DefaultConnection" automatically.
builder.Services.AddDbContext<CratesContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        // The Fix: Explicitly tell EF where the migrations live
        b => b.MigrationsAssembly("Crates.Data")
    ));

var app = builder.Build();

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