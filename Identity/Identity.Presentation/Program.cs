using Identity.Application;
using Identity.Infrastructure;
using Identity.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// using (var scope = app.Services.CreateScope())
// {
//     scope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();
// }

app.Run();