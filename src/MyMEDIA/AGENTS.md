# MyMEDIA

## Project Description
MyMEDIA is a multi-platform application for listing and selling media products.

## Structure
- **MyMEDIA.Shared**: Shared entities and DTOs.
- **MyMEDIA.API**: RESTful API with Identity and Swagger.
- **MyMEDIA.Client**: Razor Class Library containing the shared frontend UI.
- **MyMEDIA.Web**: Blazor Web App host for the frontend.
- **MyMEDIA.Management**: Store Management application for administrators.

## Requirements
- .NET 8 SDK
- SQL Server LocalDB

## Running
1. Navigate to the project folder.
2. Run `dotnet run` in the respective project directories (API, Web, Management).
