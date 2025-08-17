# Project for StackOverflow migrations and database scripts

```shell
dotnet ef migrations add Initial --project .\Auis.StackOverflow.DatabaseMigrations.csproj
```

- Don't forget to precompile models on every change

``` shell
dotnet ef dbcontext optimize --output-dir ..\Auis.StackOverflow.DataAccess\CompiledModels --namespace Auis.StackOverflow.DataAccess.Compiledmodels --project ..\Auis.StackOverflow.DataAccess\Auis.StackOverflow.DataAccess.csproj
```
