# Header

Run tests with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Generate code coverage report:

```bash
reportgenerator --settings coverlet.runsettings -reports:/Users/ali/Playground/test-dotnet-webinar/BookstoreAPI.Tests/TestResults/01b2a94f-1ad5-4973-979c-3fbf70a3ca72/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html
```
