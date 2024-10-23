FROM mcr.microsoft.com/dotnet/sdk:8.0 as BUILD

WORKDIR /app

COPY . .

RUN dotnet build HackArena2024H2-CSharp.sln -c Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=BUILD /app/MonoTanksClient/bin/Debug/net8.0/ .

COPY ./data /app/data

ENTRYPOINT [ "dotnet", "MonoTanksClient.dll" ]