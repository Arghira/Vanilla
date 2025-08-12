# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ./Vanilla.csproj
RUN dotnet publish ./Vanilla.csproj -c Release -o /app/out

# ===== run =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Vanilla.dll"]
