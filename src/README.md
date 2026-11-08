# DotNetDemo

A showcase of different modern .NET and web technologies.

* The main part of the application is still Blazor Server;
* Goal is to create some kind of notification service which checks the Azure Table Store and then trigger process to database and purge comments;
* Created an InMemoryTableServiceClient and InMemoryTableClient to mimick Azure Table Storage in memory as an experiment. Uses a static field; have to investigate thread safety. Makes it easy for this solution to run in all circumstances.

## Publishing

* dotnet publish DotNetDemo.Web/DotNetDemo.Web.csproj -o ../DotNetDemo.Web
* dotnet publish DotNetDemo.Api/DotNetDemo.Api.csproj -o ../DotNetDemo.Api
* dotnet publish DotNetDemo.Api/DotNetDemo.Worker.csproj -o ../DotNetDemo.Worker
* tar -cjvf DotnetDemo.tar.bz2 ../DotNetDemo.*
* scp -i ~/.ssh/key DotNetDemo.tar.bz2 user@host:
* rm ../DotNetDemo.*
* Remotely pdate and restart your services.

or

* Start your CI/CD pipelines.
