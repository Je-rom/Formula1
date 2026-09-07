using GitFormula_1.Data;
using GitFormula_1.Interfaces.Providers;
using GitFormula_1.Interfaces.Repository;
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Providers;
using GitFormula_1.Repositories;
using GitFormula_1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//     });
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
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IScoringService, ScoringService>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IBadgeService, BadgeService>();
builder.Services.AddScoped<IDuelService, DuelService>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GitFormula1DbContext>();
    await GitFormula_1.Seeder.BadgeSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
