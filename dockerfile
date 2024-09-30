# Stage 1: Build and Migration
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS runtime
WORKDIR /app

# Instalar a ferramenta dotnet-ef (versão compatível com .NET 6.0)
RUN dotnet tool install --global dotnet-ef --version 6.0.33

# Adicionar a ferramenta instalada ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copiar os arquivos de projeto
COPY Locacao/Locacao.csproj ./Locacao/
COPY Locacao.Repository/Locacao.Repository.csproj ./Locacao.Repository/
COPY Locacao.Service/Locacao.Service.csproj ./Locacao.Service/

# Copiar o arquivo de configuração appsettings.json para Locacao.Repository
COPY Locacao/appsettings.json ./Locacao.Repository/

RUN dotnet restore ./Locacao/Locacao.csproj


# Copiar o restante dos arquivos
COPY . .

RUN dotnet restore ./Locacao/Locacao.csproj
RUN dotnet build ./Locacao/Locacao.csproj -c Release -o /app/build
RUN dotnet publish ./Locacao/Locacao.csproj -c Release -o /app/publish


# Stage 2: Runtime
WORKDIR /app/publish

# Executar migrações e rodar a aplicação
CMD ["sh", "-c", "until dotnet ef database update --project ../Locacao.Repository/Locacao.Repository.csproj; do echo 'Waiting for DB...'; sleep 5; done && dotnet Locacao.dll"]