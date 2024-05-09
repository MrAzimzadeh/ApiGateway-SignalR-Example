using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed((host) => true) 
                .AllowCredentials();
        });

    
});

// Ocelat konifguration
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Required for WebSockets
app.UseWebSockets();
app.UseOcelot().Wait(); // Required for Ocelot
app.Run();
