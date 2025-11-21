var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddConnectionString("DefaultConnection");

builder.AddProject<Projects.Crates_API>("crates-api")
    .WithReference(postgres);

builder.Build().Run();
