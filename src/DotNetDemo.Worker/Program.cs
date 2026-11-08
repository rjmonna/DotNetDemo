using DotNetDemo.Services;
using DotNetDemo.Services.Contracts;
using DotNetDemo.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var apiUrl = builder.Configuration.GetValue<string>("ApiUrl") ?? throw new InvalidOperationException("Configuration of ApiUrl is missing.");

builder.Services.AddHttpClient<IArticleCommentService, ArticleCommentService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

var host = builder.Build();
host.Run();
