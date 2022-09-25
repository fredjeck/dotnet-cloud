# Adding Code Coverage

The basic XUnit project template already comes with the code coverage collector configured in the csrpoj file.


## Installing the report generator tool

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

## Running code coverage reports

The `.vscode/tasks.json` file contains two tasks, one to generate the coverage report, and the other one to generate an HTML report out of it.
After running the first task, move the `coverage.cobertura.xml` file up one level before generating the HTML report.
Please check the tasks file for command line arguments.

## Going further

You can read more about code coverage in the [Official Documentation](https://learn.microsoft.com/fr-fr/dotnet/core/testing/unit-testing-code-coverage?tabs=windows)