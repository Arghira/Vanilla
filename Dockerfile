# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copiem tot repo-ul
COPY . .

# restore + publish (ajustează calea dacă proiectul tău are alt nume/folder)
RUN dotnet restore
RUN dotnet publish ./Vanilla/Vanilla.csproj -c Release -o /app/out

# ===== run =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Vanilla.dll"]
