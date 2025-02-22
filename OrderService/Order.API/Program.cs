using Order.Application.DependencyInjection;
using Order.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructureService();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
