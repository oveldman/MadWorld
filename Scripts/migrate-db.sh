#!/bash/sh
dotnet ef migrations add InitialCreate
dotnet ef migrations add InitialCreate --context AuthenticationContext
dotnet ef database update
dotnet ef database update --context AuthenticationContext
dotnet ef migrations remove