using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PatikaOdev1.Controllers
{
    public class WeatherForecast
    {
        public WeatherForecast() { }
        public WeatherForecast(DateTime d, int tc, string s)
        {
            Date = d;
            TemperatureC = tc;
            Summary = s;
        }

        [Required]
        public DateTime? Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public static class staticData
    {
        public static List<WeatherForecast> forecastList = new List<WeatherForecast>() { {new WeatherForecast(new DateTime(2) ,2,"cold") },
                                                                                            {new WeatherForecast(new DateTime(1) ,1,"warm") }};
    }

    //[Authorize(Roles = "Member")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        /*private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };*/

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

        }

        [HttpGet("GetWeatherForecastOrdered")]
        public IEnumerable<WeatherForecast> GetOrdered()
        {
            return staticData.forecastList.OrderBy(t => t.TemperatureC);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return staticData.forecastList;
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public IEnumerable<WeatherForecast> Post([FromQuery] WeatherForecast wf)
        {
            staticData.forecastList.Add(wf);

            return staticData.forecastList;
        }

        [HttpPut(Name = "PutWeatherForecast")]
        public IEnumerable<WeatherForecast> Put(int id, WeatherForecast wf)
        {
            staticData.forecastList[id] = wf;

            return staticData.forecastList;
        }

        [HttpPatch(Name = "PatchWeatherForecast")]
        public IEnumerable<WeatherForecast> Patch(int id, WeatherForecast wf)
        {
            foreach (PropertyInfo propertyInfo in wf.GetType().GetProperties())
            {
                /*var b = wf.GetType().GetProperty(propertyInfo.Name);
                var a = wf.GetType().GetProperty(propertyInfo.Name).GetValue(wf); debugging purpose*/
                if (wf.GetType().GetProperty(propertyInfo.Name).GetValue(wf) != null && wf.GetType().GetProperty(propertyInfo.Name).CanWrite)
                {
                    staticData.forecastList[id].GetType().GetProperty(propertyInfo.Name).SetValue(staticData.forecastList[id], wf.GetType().GetProperty(propertyInfo.Name).GetValue(wf));
                }
            }

            return staticData.forecastList;
        }

        [HttpDelete(Name = "PutWeatherForecast")]
        public IEnumerable<WeatherForecast> Delete(int id)
        {
            staticData.forecastList.RemoveAt(id);
            return staticData.forecastList;
        }
    }
}