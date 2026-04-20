FROM mcr.microsoft.com/mssql/server:2022-latest AS database

WORKDIR /app

RUN dotnet ef migrations add InitialCreate -o Migrations
