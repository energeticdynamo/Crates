var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImage("pgvector/pgvector", "pg16")
    .WithPgAdmin()
    .AddDatabase("DefaultConnection");

builder.AddProject<Projects.Crates_API>("crates-api")
    .WithReference(postgres);

builder.Build().Run();
