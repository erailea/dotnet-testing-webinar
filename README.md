# Header

Run tests with code coverage:

```bash
dotnet test --settings .runsettings --collect:"XPlat Code Coverage"
```

Generate code coverage report:

```bash
reportgenerator --settings coverlet.runsettings -reports:BookstoreAPI.Tests/TestResults/f5e846a9-01d8-4593-8e53-67e42269079a/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html
```
