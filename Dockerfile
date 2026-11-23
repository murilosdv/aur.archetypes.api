# syntax=docker/dockerfile:1

ARG SOLUTION=Archetypes.Api.slnx

## Restore: packages and tools, in a layer that only changes with the project files ##
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS restore
ARG SOLUTION
WORKDIR /src

# Outside /root, so the user of the test stage can read them.
ENV NUGET_PACKAGES=/opt/nuget
ENV PATH="$PATH:/opt/tools"

COPY global.json Directory.Build.props Directory.Packages.props $SOLUTION ./
COPY --parents src/*/*.csproj tests/*/*.csproj tools/*/*.csproj ./
RUN dotnet restore "$SOLUTION"
RUN dotnet tool install --tool-path /opt/tools dotnet-reportgenerator-globaltool --version 5.*

## Build: the sources ##
FROM restore AS build
COPY . .

## Test: every test project, then the coverage report; not as root ##
FROM build AS test

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
    "-assemblyfilters:+Archetypes.Api;+Aurore.Archetypes.*;-Aurore.Archetypes.Api.Tools.Migrations" \
    "-classfilters:-Aurore.Archetypes.Api.App.Api.Playground.*;-Program"

cat artifacts/coverage-report/Summary.txt
SCRIPT

# uid 1000 is the user of the machine, so it can write to the mounted artifacts folder.
RUN chmod +x run-tests.sh && chown -R 1000:1000 /src /opt/nuget
ENV DOTNET_CLI_HOME=/tmp
USER 1000:1000

ENTRYPOINT ["./run-tests.sh"]

## Publish: the application ##
FROM build AS publish
RUN dotnet publish src/App.Api/App.Api.csproj --configuration Release --no-restore --output /app

## Runtime: the application on the ASP.NET image, not as root ##
FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS runtime
WORKDIR /app
COPY --from=publish /app .

EXPOSE 8080
USER app

ENTRYPOINT ["dotnet", "Archetypes.Api.dll"]
