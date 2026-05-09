using System.Text.Json;
using DistanceCalculator.Options;
using DistanceCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
    options.UseUtcTimestamp = true;
    options.JsonWriterOptions = new JsonWriterOptions { Indented = false };
});

builder.Services.Configure<TwoGisOptions>(builder.Configuration.GetSection(TwoGisOptions.SectionName));

builder.Services.AddSingleton<HaversineDistanceCalculator>();

builder.Services.AddSingleton<TwoGisDistanceCalculator>();
builder.Services.AddHttpClient<TwoGisDistanceCalculator>();

builder.Services.AddSingleton<ICalculatorFactory, CalculatorFactory>();

builder.Services.AddScoped<IDistanceService, DistanceService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<DistanceCalculator.Swagger.DistanceRequestExampleFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();