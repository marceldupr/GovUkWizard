# GovUk Wizard Prototype

A generic 'Survey', 'Wizard' using the GovUK toolkit. This metadata driven application can easily be used to drive the UI by code and enable rapid development of surveys or data gathering 'Wizards'.
This includes executing validation and even business rules with outcomes after the survey is completed. 
 
## Prerequisites
- Visual Studio 2019
- .NET Core 3.1 SDK
- Node
- NPM
- Python 2.7
- Ruby

## Set up
Install the frontend dependencies by running the following from the project folder:
```
npm install
```
Build the solution, either within Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet build
```
Build the frontend assets (compiled JS and CSS) by running the following from the project folder:
```
npm buildDev
```
Run the web application through Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet run
```
