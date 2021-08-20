run:
	@echo "Runing rest API"
	@dotnet run --project RestApi/RestApi.csproj

test:
	@echo "Runing rest API tests"
	@dotnet test RestApiTest/RestApiTest.csproj

