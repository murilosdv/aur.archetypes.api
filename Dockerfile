# syntax=docker/dockerfile:1

ARG SOLUTION=Archetypes.Api.slnx

FROM mcr.microsoft.com/dotnet/sdk:11.0 AS restore
ARG SOLUTION
ARG NUGET_SOURCE=https://nuget.pkg.github.com/murilosdv/index.json
ARG NUGET_USER
WORKDIR /src

COPY global.json Directory.Build.props $SOLUTION ./
COPY --parents src/*/*.csproj tests/*/*.csproj tools/*/*.csproj ./
RUN --mount=type=secret,id=nuget_token \
    dotnet nuget add source "$NUGET_SOURCE" --name aurore --username "$NUGET_USER" --password "$(cat /run/secrets/nuget_token)" --store-password-in-clear-text \
    && dotnet restore "$SOLUTION" \
    && dotnet nuget remove source aurore

FROM restore AS build
COPY . .

FROM build AS test
RUN dotnet tool install -g dotnet-reportgenerator-globaltool --version 5.*
ENV PATH="$PATH:/root/.dotnet/tools"

COPY <<'SCRIPT' run-tests.sh
#!/usr/bin/env bash
set -euo pipefail

for project in Tests Tests.Integration; do
    dotnet test "tests/$project" --no-restore --configuration Release \
        --coverage --coverage-output-format cobertura --coverage-output "$project.cobertura.xml"
done

reportgenerator \
    "-reports:artifacts/test-results/*.cobertura.xml" \
    "-targetdir:artifacts/coverage-report" \
    "-reporttypes:Html;TextSummary" \
    "-assemblyfilters:+Archetypes.Api;+Aurore.Archetypes.*;-Aurore.Archetypes.Api.Tools.Migrations"

cat artifacts/coverage-report/Summary.txt
SCRIPT
RUN chmod +x run-tests.sh
ENTRYPOINT ["./run-tests.sh"]

FROM build AS publish
RUN dotnet publish src/App.Api/App.Api.csproj --configuration Release --no-restore --output /app

FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS runtime
WORKDIR /app
COPY --from=publish /app .

EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "Archetypes.Api.dll"]
