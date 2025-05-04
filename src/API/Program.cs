using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using Persistence;
using Persistence._Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var databaseCfgSection = builder.Configuration.GetSection("Database");
    var connectionBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseCfgSection.GetValue<string>("Host"),
        Port = databaseCfgSection.GetValue("Port", 5432),
        Database = databaseCfgSection.GetValue<string>("Database"),
        Username = databaseCfgSection.GetValue<string>("Username"),
        Password = databaseCfgSection.GetValue<string>("Password")
    };
    options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
    options.UseNpgsql(connectionBuilder.ToString());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();    
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("", () => Results.Redirect("scalar/v1"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();