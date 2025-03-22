using Data;
using Buisness;
using Microsoft.Build.Framework;
using Buisness.Services.Auth;
using Buisness.Services.Background;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddDataAccess();
builder.Services.AddBuisnessLogic();
builder.Services.AddAuth(builder.Configuration);
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddControllers();
builder.Services.AddHostedService<AutoDelete>();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();





