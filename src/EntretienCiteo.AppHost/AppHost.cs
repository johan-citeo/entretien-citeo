var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb();

var db = postgres.AddDatabase("citeo-db");

builder.AddProject<Projects.EntretienCiteo_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
