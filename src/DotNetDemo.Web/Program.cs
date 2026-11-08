using BlazorApplicationInsights;
using DotNetDemo.Services;
using DotNetDemo.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

var apiUrl = builder.Configuration.GetValue<string>("ApiUrl") ?? throw new InvalidOperationException("Configuration of ApiUrl is missing.");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<IEmployeeService, EmployeeService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<IDepartmentService, DepartmentService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<IArticleService, ArticleService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});
builder.Services.AddHttpClient<IArticleCommentService, ArticleCommentService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddBlazorApplicationInsights();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
