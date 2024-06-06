var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Auis_ApiService>("apiservice");

builder.AddProject<Projects.Auis_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
