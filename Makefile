CONNECTION ?= Host=127.0.0.1;Database=archetypes-api;Username=postgres;Password=postgres
MIGRATIONS := tools/Tools.Migrations
NAME := $(word 2,$(MAKECMDGOALS))

.PHONY: test test-local test-unit report clean migration-add migration-remove migration-script migration-check db-update db-drop db-reset

test:
	docker compose --profile tests run --build --rm tests

test-local:
	dotnet test --solution Archetypes.Api.slnx

test-unit:
	dotnet test --project tests/Tests/Tests.csproj

report:
	cmd.exe /c start artifacts\coverage-report\index.html

clean:
	rm -rf artifacts

migration-add:
	dotnet ef migrations add "$(or $(NAME),InitialDb)" --project $(MIGRATIONS) --output-dir History -- "$(CONNECTION)"

migration-remove:
	dotnet ef migrations remove --project $(MIGRATIONS) -- "$(CONNECTION)"

migration-script:
	dotnet ef migrations script --project $(MIGRATIONS) -- "$(CONNECTION)"

migration-check:
	dotnet ef migrations has-pending-model-changes --project $(MIGRATIONS) -- "$(CONNECTION)"

db-update:
	dotnet ef database update --project $(MIGRATIONS) -- "$(CONNECTION)"

db-drop:
	dotnet ef database drop --project $(MIGRATIONS) --force -- "$(CONNECTION)"

# While the model is in development there is a single migration, InitialDb: drop the database, forget every migration,
# create InitialDb again from the model and apply it.
db-reset:
	-dotnet ef database drop --project $(MIGRATIONS) --force -- "$(CONNECTION)"
	-while dotnet ef migrations remove --project $(MIGRATIONS) -- "$(CONNECTION)"; do :; done
	dotnet ef migrations add InitialDb --project $(MIGRATIONS) --output-dir History -- "$(CONNECTION)"
	dotnet ef database update --project $(MIGRATIONS) -- "$(CONNECTION)"

%:
	@:
