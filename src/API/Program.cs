using API._Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;
using Persistence;
using Persistence._Utils;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString: builder.Configuration.GetDbConnectionString().ToString(), 
        name: "postgresql", 
        tags: ["db", "sql", "postgresql"]);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionBuilder = builder.Configuration.GetDbConnectionString();
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
    app.MapGet("", () => Results.Redirect("scalar/v1"))
        .ExcludeFromDescription();
}

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();