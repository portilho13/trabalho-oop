using trabalho_oop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<Logger>(new Logger("./fms/logs/app.log"));
builder.Services.AddSingleton<Fleet>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<Logger>();
    var fleet = new Fleet(logger);
    fleet.LoadFleet(); // Load the fleet once at application startup
    return fleet;
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();