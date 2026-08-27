using GitFormula_1.Data;
using GitFormula_1.Interfaces.Providers;
using GitFormula_1.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("GitHub", client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "GitFormula1-App");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["GitHub:Token"]}");
    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
});

builder.Services.AddDbContext<GitFormula1DbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDatabase"));
});

builder.Services.AddScoped<IGitHubApiProvider, GitHubApiProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
