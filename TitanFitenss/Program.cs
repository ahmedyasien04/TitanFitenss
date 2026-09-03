using TitanFitenss.Api.Middleware;
using TitanFitenss.Application;
using TitanFitenss.Infrastructure;

var builder=WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title="Titan Fitness API",
        Version="v1",
        Description="Staff portal Titan Fitness."
    });
});
builder.Services.AddCors(options=>
{
    options.AddPolicy("AllowAll", policy=>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app=builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options=>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Titan Fitness API v1");
    });
}
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
