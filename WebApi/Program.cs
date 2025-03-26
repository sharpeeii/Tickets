using Data;
using Business;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Business.Services.Auth;
using Business.Services.Background;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddDataAccess();
builder.Services.AddBuisnessLogic();
builder.Services.AddAuth(builder.Configuration);
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddControllers();
builder.Services.AddHostedService<SessionAutoDelete>();
builder.Services.AddSwaggerGen();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();





