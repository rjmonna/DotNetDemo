using System.Text.Json.Serialization;
using DotNetDemo.Infrastructure;
using DotNetDemo.Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Azure.Data.Tables;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });;

// using Microsft.EntityFrameworkCore;
builder.Services.AddDbContext<AppDbContext>(options =>
    // options.UseSqlServer(
    //     builder.Configuration.GetConnectionString("Default")
    // )

    options.UseInMemoryDatabase("Default")
);
builder.Services.AddDbContext<AppDbContext>(options =>
    // options.UseSqlServer(
    //     builder.Configuration.GetConnectionString("Default")
    // )

    options.UseInMemoryDatabase("Default")
);
builder.Services.AddDbContext<AppSecondDbContext>(options =>
    // options.UseSqlServer(
    //     builder.Configuration.GetConnectionString("Default")
    // )

    options.UseInMemoryDatabase("Default")
);

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleCommentRepository, ArticleCommentRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IAzureTableStorage, AzureTableStorage>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});

// builder.Services.AddAzureClients(clientBuilder =>
// {
//     clientBuilder.AddTableServiceClient(
//         builder.Configuration.GetSection("ArticleCommentStorage")
//     );
// });

builder.Services.AddScoped<TableServiceClient, InMemoryTableServiceClient>();

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);

    var secondContext = services.GetRequiredService<AppSecondDbContext>();
    secondContext.Database.EnsureCreated();
    DbInitializer.Initialize(secondContext);

    var tableServiceClient = services.GetRequiredService<TableServiceClient>();

    if (tableServiceClient is InMemoryTableServiceClient)
    {
        await DbInitializer.InitializeAsync((InMemoryTableServiceClient)tableServiceClient);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();