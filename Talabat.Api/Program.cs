using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using Talabat.Api.Extensions;
using Talabat.Api.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Service;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationServices();

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(connection!);
});

builder.Services.AddCors();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "SampleInstance";
});

builder.Services.AddSingleton<IDistributedRedisCacheService, DistributedRedisCacheService>();

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

    var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityDbContext.Database.MigrateAsync();

    #region Seed database
    await StoreContextSeed.SeedDatabaseAsync(dbContext);
    #endregion
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await AppIdentityDbContextSeed.SeedUserAsync(userManager);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An Error Occured During Apply The Migrations");
}
#endregion

#region Configure the HTTP request pipeline. 
app.UseMiddleware<ExceptionHandingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerMiddlewares();
}

app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(options =>
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
