var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AllUsefulInformationSearch_ApiService>("apiservice");

builder.AddProject<Projects.AllUsefulInformationSearch_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
