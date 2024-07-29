using DatabaseVariables;

var myCorsPolicy = "_allowCertainOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => {
    options.AddPolicy(name: myCorsPolicy, policy => {
        policy.WithOrigins("http://localhost:4200");
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<DatabaseCredentials>(builder.Configuration.GetSection("neonCredentials"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(myCorsPolicy);
app.UseAuthorization();
app.MapControllers();
app.Run();