# Etapa 1 - Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj e restaura dependências
COPY *.csproj .
RUN dotnet restore

# Copia o restante do código e compila
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia build final
COPY --from=build /app/publish .

# Expõe porta padrão
EXPOSE 8080

# Variável usada pelo Render
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# SQLite: garantir que o diretório de dados exista
RUN mkdir -p /app/Data

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CupcakeStore.dll"]
