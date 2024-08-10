# Header

Run tests with code coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Generate code coverage report:

```bash
reportgenerator --settings coverlet.runsettings -reports:{{coverage.cobertura.xml file path}} -targetdir:coverage-report -reporttypes:Html
```
