using Data;
using Business;
using Microsoft.EntityFrameworkCore;
using Business.Services.Auth;
using Business.Services.Background;
using Scalar.AspNetCore;
using WebApi.Middleware;
using WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddDataAccess();
builder.Services.AddBuisnessLogic();
builder.Services.AddAuth(builder.Configuration);
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddControllers();
builder.Services.AddHostedService<SessionAutoDelete>();
builder.Services.AddSwaggerWithJwt();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http);
});
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    
    c.ConfigObject.AdditionalItems["oauth2RedirectUrl"] = "/swagger/oauth2-redirect.html";
    c.ConfigObject.AdditionalItems["persistAuthorization"] = "true";
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();





