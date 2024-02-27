using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
