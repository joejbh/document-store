using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace document_store.Controllers;



[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public UploadController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    // the HTTP post request.The Body size limit is disabled
    [HttpPost, DisableRequestSizeLimit]
    public IActionResult Post()
    {
        try
        {
            var postedFile = Request.Form.Files[0];
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            Console.WriteLine("HI2");

            if (postedFile.Length > 0)
            {
                // 3a. read the file name of the received file
                var fileName = ContentDispositionHeaderValue
                                .Parse(postedFile.ContentDisposition).FileName
                                .Trim('"');
                // 3b. save the file on Path
                var finalPath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(finalPath, FileMode.Create))
                {
                    postedFile.CopyTo(fileStream);
                }
                return Ok($"File is uploaded Successfully");
            }
            else
            {
                return BadRequest("The File is not received.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Some Error Occcured while uploading File {ex.Message}");
        }
    }
}