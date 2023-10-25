FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./ .
RUN dotnet restore

WORKDIR /app/AllUsefulInformationSearch
RUN dotnet publish -c release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 80
EXPOSE 443
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "AllUsefulInformationSearch.dll"]
