﻿# Project for Wikipedia migrations and database scripts

- How to add migrations to separate project (https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli)

- Don't forget to precompile models on every change
``` shell
dotnet ef dbcontext optimize --output-dir ..\Auis.Wikipedia.DataAccess\CompiledModels --namespace Auis.Wikipedia.DataAccess.Compiledmodels
```

- Don't forget to set correct DOTNET_ENVIRONMENT variable