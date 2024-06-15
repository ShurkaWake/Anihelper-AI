using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Net.Http;
using MongoDB.Driver.GridFS;
using Microsoft.AspNetCore.Builder;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongoConnectionString = builder.Configuration["DbConnectionString"];
var mongoClient = new MongoClient(mongoConnectionString);
var databaseName = "anihelper";
var database = mongoClient.GetDatabase(databaseName);


builder.Services.AddSingleton(database);
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/video", 
    async ([FromBody] CreationModel video,
    [FromServices] IMongoDatabase database,
    [FromServices] IHttpClientFactory httpClientFactory,
    [FromServices] IConfiguration configuration) =>
{
    var httpClient = httpClientFactory.CreateClient();
    var gridFsBucket = new GridFSBucket(database);
    var key = configuration["Key"];
    try
    {
        if (key != video.key)
        {
            return Results.StatusCode(403);
        }
        var response = await httpClient.GetAsync(video.url);
        if (!response.IsSuccessStatusCode)
        {
            return Results.BadRequest("Failed to fetch video from the provided URL.");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var filename = Path.GetFileName(video.url); // Extract filename from URL

        var options = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
                    {
                        { "contentType", response.Content.Headers.ContentType.MediaType }
                    }
        };

        var fileId = await gridFsBucket.UploadFromStreamAsync(filename, stream, options);
        return Results.Created("/video", new {id = fileId.ToString()});
    }
    catch (Exception ex)
    {
        // Handle exceptions (e.g., invalid URL, network issues, etc.)
        return Results.StatusCode(500);
    }
})
.WithName("UploadVideo")
.WithOpenApi();

app.MapGet("/video/{videoId}", 
    async([FromRoute] string videoId,
    [FromServices] IMongoDatabase database) => 
{
    var gridFsBucket = new GridFSBucket(database);
    try
    {
        var objectId = ObjectId.Parse(videoId);
        var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.IdAsBsonValue, BsonValue.Create(objectId));
        var fileInfo = await gridFsBucket.Find(filter).FirstOrDefaultAsync();

        if (fileInfo == null)
        {
            return Results.NotFound();
        }

        var stream = await gridFsBucket.OpenDownloadStreamAsync(objectId);
        return Results.File(stream, "video/mp4");
    }
    catch (Exception ex)
    {
        // Handle exceptions (e.g., invalid ObjectId, database connection issues, etc.)
        return Results.StatusCode(500);
    }
})
.WithName("GetVideo")
.WithOpenApi();

app.Run();

internal record CreationModel(string url, string key);
