var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithDataVolume();

var db = postgres.AddDatabase("citeo-db");

var api = builder.AddProject<Projects.EntretienCiteo_Api>("api")
    .WithReference(db)
    .WaitFor(db);

api.WithUrl("/swagger", "Swagger");

builder.Build().Run();