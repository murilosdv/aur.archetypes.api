-include .env

# Git Bash would turn the /k: and /d: arguments of the scanner into paths
export MSYS2_ARG_CONV_EXCL := *

# Project
SOLUTION := Archetypes.Api.slnx
ARTIFACTS := artifacts
TEST_RESULTS := $(ARTIFACTS)/test-results
COVERAGE_REPORT := $(ARTIFACTS)/coverage-report/index.html

# Dev-sbx
SBX_DIR ?= ../.dev-sbx
SBX := $(MAKE) -C $(SBX_DIR)

# SonarQube (SONAR_TOKEN is a secret: .env)
SONAR_HOST ?= http://127.0.0.1:9000
SONAR_PROJECT_NAME ?= aur.archetypes.api
SONAR_COVERAGE := $(TEST_RESULTS)/*.xml
SONAR_COVERAGE_EXCLUSIONS := src/App.Api/Playground/**,src/App.Api/Program.cs
SONAR_EXCLUSIONS := tools/Tools.Migrations/History/**
SONAR_BEGIN := dotnet sonarscanner begin /k:"$(SONAR_PROJECT_NAME)" /d:sonar.host.url="$(SONAR_HOST)" /d:sonar.token="$(SONAR_TOKEN)" /d:sonar.cs.vscoveragexml.reportsPaths="$(SONAR_COVERAGE)" /d:sonar.exclusions="$(SONAR_EXCLUSIONS)" /d:sonar.coverage.exclusions="$(SONAR_COVERAGE_EXCLUSIONS)"
SONAR_END := dotnet sonarscanner end /d:sonar.token="$(SONAR_TOKEN)"
XML_COVERAGE := --coverage --coverage-output-format xml

# Migrations and database
CONNECTION ?= Host=127.0.0.1;Database=archetypes-api;Username=postgres;Password=postgres
MIGRATIONS := tools/Tools.Migrations
HISTORY := History
INITIAL := InitialDb
NAME := $(word 2,$(MAKECMDGOALS))
EF := dotnet ef
EF_MIGRATIONS := $(EF) migrations
EF_DATABASE := $(EF) database
EF_ARGS := --project $(MIGRATIONS)
EF_CONNECTION := -- "$(CONNECTION)"

.PHONY: sbx-up sbx-down sonar test test-local test-unit report clean migration-add migration-remove migration-script migration-check db-update db-drop db-reset

## Dev-sbx ##

sbx-up:
	$(SBX) docker-up

sbx-down:
	$(SBX) docker-down

## SonarQube: starts the dev-sbx with SonarQube and leaves it running; make sonar-down (dev-sbx) removes it ##

sonar:
	$(SBX) docker-up PROFILE=sonar
	rm -rf $(TEST_RESULTS)
	@$(SONAR_BEGIN)
	dotnet build $(SOLUTION) --no-incremental
	dotnet test --solution $(SOLUTION) $(XML_COVERAGE)
	@$(SONAR_END)

## Tests ##

test:
	docker compose --profile tests run --build --rm tests

test-local:
	dotnet test --solution $(SOLUTION)

test-unit:
	dotnet test --project tests/Tests/Tests.csproj

report:
	cmd.exe /c start $(subst /,\,$(COVERAGE_REPORT))

## Clean ##

clean:
	rm -rf $(ARTIFACTS)
	find . -path './.git' -prune -o -type d \( -name bin -o -name obj \) -prune -exec rm -rf {} +

## Migrations ##

migration-add:
	$(EF_MIGRATIONS) add "$(or $(NAME),$(INITIAL))" $(EF_ARGS) --output-dir $(HISTORY) $(EF_CONNECTION)

migration-remove:
	$(EF_MIGRATIONS) remove $(EF_ARGS) $(EF_CONNECTION)

migration-script:
	$(EF_MIGRATIONS) script $(EF_ARGS) $(EF_CONNECTION)

migration-check:
	$(EF_MIGRATIONS) has-pending-model-changes $(EF_ARGS) $(EF_CONNECTION)

## Database ##

db-update:
	$(EF_DATABASE) update $(EF_ARGS) $(EF_CONNECTION)

db-drop:
	$(EF_DATABASE) drop $(EF_ARGS) --force $(EF_CONNECTION)

# One migration, InitialDb, while the model is in development: drop, forget every migration, create it again and apply it
db-reset:
	-$(EF_DATABASE) drop $(EF_ARGS) --force $(EF_CONNECTION)
	-while $(EF_MIGRATIONS) remove $(EF_ARGS) $(EF_CONNECTION); do :; done
	$(EF_MIGRATIONS) add $(INITIAL) $(EF_ARGS) --output-dir $(HISTORY) $(EF_CONNECTION)
	$(EF_DATABASE) update $(EF_ARGS) $(EF_CONNECTION)

%:
	@:
