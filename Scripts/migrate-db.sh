#!/bash/sh
dotnet ef migrations add InitialCreate
dotnet ef migrations add InitialCreate --context AuthenticationContext
dotnet ef migrations add InitialCreate --context MadWorldContext
dotnet ef database update
dotnet ef database update --context AuthenticationContext
dotnet ef database update --context MadWorldContext
dotnet ef migrations remove