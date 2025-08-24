# AllUsefulInformationSearch

Check some info in README.md https://github.com/vgotra/AllUsefulInformationSearch/blob/main/Auis.StackOverflow.App/README.md

Pet project for keeping all useful information from the useful information providers, with possible incremental updates and maybe integration with AI.

Also for improving Architecture, Technical full stack, Devops, Cloud skills.

Can be reused to improve SDLC understanding (development, TDD, refactorings, prototyping, etc).

**Notes:**

For local Loki:
```shell
docker-compose -f docker-compose-loki.yml up -d
```

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo
    .LokiHttp(new NoAuthCredentials("http://localhost:3100"))
    .CreateLogger();
```