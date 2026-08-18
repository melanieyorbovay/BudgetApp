FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Docker réutilise le cache et saute le restore NuGet.
COPY BudgetApp/BudgetApp.csproj BudgetApp/
RUN dotnet restore BudgetApp/BudgetApp.csproj

COPY BudgetApp/ BudgetApp/
RUN dotnet publish BudgetApp/BudgetApp.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

USER $APP_UID

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BudgetApp.dll"]