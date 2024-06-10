using AspNetCoreRateLimit;
using EasyPayChallenge.Infrastructure.DB;
using EasyPayChallenge.Infrastructure.Extensions;
using EasyPayChallenge.WebApi.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediator();

builder.Services.AddRepositories(builder.Configuration);
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
DatabaseInitializer.InitializeDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));
var redisConfiguration = builder.Configuration.GetValue<string>("Redis:Configuration");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();