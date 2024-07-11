using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using Talabat.Api.Extensions;
using Talabat.Api.Middlewares;
using Talabat.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddApplicationServices();

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(connection!);
});

builder.Services.AddCors();
#endregion

var app = builder.Build();

#region Update Database
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var dbContext = services.GetRequiredService<StoreContext>();
    await dbContext.Database.MigrateAsync();
    #region Seed database
    await StoreContextSeed.SeedDatabaseAsync(dbContext);
    #endregion
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An Error Occured During Apply The Migrations");
}
#endregion

#region Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionHandingMiddleware>();
    app.UseSwaggerMiddlewares();
}

app.UseCors(options =>
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
