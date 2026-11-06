# Copilot Instructions

## Build and run

- Restore and build the full solution with `dotnet restore BlazorServerCRUD.slnx` and `dotnet build BlazorServerCRUD.slnx`.
- Run the API with `dotnet run --project BlazorServerCRUD.Api/BlazorServerCRUD.Api.csproj --urls http://localhost:5000`. Run the UI separately with `dotnet run --project BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj`; its development `ApiUrl` points to port 5000.
- Publish the UI with `dotnet publish BlazorServerCRUD.Web/BlazorServerCRUD.Web.csproj`. The VS Code `build`, `publish`, and `watch` tasks are defined in `.vscode/tasks.json`.
- No test projects or lint commands are currently configured. When a test project is added, run its suite with `dotnet test <test-project>.csproj` or a single test with `dotnet test <test-project>.csproj --filter FullyQualifiedName~<TestName>`.

## Architecture

- The solution has three projects, all targeting .NET 10: `BlazorServerCRUD.Models` shares the `Department`, `Employee`, and `Gender` types; `BlazorServerCRUD.Api` exposes CRUD controllers and accesses data through repository interfaces and EF Core; `BlazorServerCRUD.Web` is a server-side Blazor UI that calls the API through typed HTTP services.
- API routes follow `api/[controller]` (`api/department` and `api/employee`). Keep UI data access in `BlazorServerCRUD.Web/Services` rather than having components call the API directly.
- The API currently configures EF Core's in-memory provider and seeds sample departments and employees at startup. This data is not durable across process restarts. The SQL Server provider setup is commented out in `BlazorServerCRUD.Api/Program.cs`.
- The shared EF entities have a bidirectional Department/Employee navigation. The API configures JSON cycle handling in `Program.cs`; preserve that behavior when changing serialization or response shapes.

## Repository conventions

- Configure the UI's API endpoint through `ApiUrl` (development default: `http://localhost:5000` in `BlazorServerCRUD.Web/appsettings.Development.json`); the typed `HttpClient` services use it as their base address.
- Repository interfaces and EF implementations live together under `BlazorServerCRUD.Api/Models`. Controllers delegate persistence operations to those repositories.
- Blazor CRUD pages are grouped by entity under `BlazorServerCRUD.Web/Pages/Department` and `BlazorServerCRUD.Web/Pages/Employee`; each group uses a shared `Form.razor` component for create/update forms.
- See `README.md` for the solution's origin and its in-memory database setup.
