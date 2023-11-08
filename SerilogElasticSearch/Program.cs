using Nest;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var node = new Uri("http://localhost:9200");

// Elasticsearch bağlantısı oluşturun
var settings = new ConnectionSettings(node)
    .DefaultIndex("log") // Kullandığınız index adını belirtin
    .DisableDirectStreaming()
    .PrettyJson();

var client = new ElasticClient(settings);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions
    {
        IndexFormat = "log-{0:yyyy.MM.dd}", // İndeks formatını belirtin, tarih bilgisi ile indeksler oluşturulur
    })
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Log.Information("deneme");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
