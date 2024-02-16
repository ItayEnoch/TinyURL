using TinyURL;
using TinyURL.Repositories;
using TinyURL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var settings = new Settings();
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<IUrlRepository, UrlRepository>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<HandleUrlService>();
builder.Services.AddSingleton<RedirectService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
