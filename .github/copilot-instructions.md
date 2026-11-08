# Copilot Instructions

## Build and run

- Restore and build the full solution with `dotnet restore BlazorServerCRUD.slnx` and `dotnet build BlazorServerCRUD.slnx`.
- Run the API with `dotnet run --project src/DotNetDemo.Api/DotNetDemo.Api.csproj --urls http://localhost:5000`. Run the UI separately with `dotnet run --project src/DotNetDemo.Web/DotNetDemo.Web.csproj`; its development `ApiUrl` points to port 5000.
- Publish the UI with `dotnet publish src/DotNetDemo.Web/DotNetDemo.Web.csproj`. VS Code build, publish, and watch tasks are in `src/.vscode/tasks.json`.
- No test projects or lint commands are currently configured. When a test project is added, run its suite with `dotnet test <test-project>.csproj` or a single test with `dotnet test <test-project>.csproj --filter FullyQualifiedName~<TestName>`.

## Architecture

- The solution contains API, Infrastructure, Models, Services, Web, and Worker projects under `src`, all targeting .NET 10.
- `DotNetDemo.Models` contains shared entities and API DTOs. `DotNetDemo.Api` exposes CRUD controllers and maps entities to DTOs; persistence is implemented in `DotNetDemo.Infrastructure`.
- API routes follow `api/[controller]`. Keep UI data access in `DotNetDemo.Services`; the Blazor UI registers typed HTTP clients against the configured `ApiUrl`.
- The API configures EF Core's in-memory provider and seeds sample data at startup. This data is not durable across process restarts. The SQL Server provider setup is commented out in `src/DotNetDemo.Api/Program.cs`.
- The Department/Employee entities have bidirectional navigation. Preserve the API's JSON cycle handling when changing serialization or response shapes.

## Repository conventions

- Configure the UI API endpoint through `ApiUrl` (development default: `http://localhost:5000` in `src/DotNetDemo.Web/appsettings.Development.json`).
- Repository interfaces and EF implementations live under `src/DotNetDemo.Infrastructure`; API controllers delegate persistence operations to those repositories.
- Blazor CRUD pages are grouped under `src/DotNetDemo.Web/Pages/Department`, `Employee`, and `Article`; Department and Employee forms use shared `Form.razor` components.
- See `src/README.md` for the solution's background and in-memory storage setup.
