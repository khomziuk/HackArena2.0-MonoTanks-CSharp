FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet build HackArena2.0-MonoTanks-CSharp.sln -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/MonoTanksBot/bin/Release/net8.0/ .

COPY ./data /app/data

ENTRYPOINT [ "dotnet", "MonoTanksBot.dll" ]