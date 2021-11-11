using System.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WeatherForecast
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiurl = ConfigurationManager.AppSettings["APIurl"];
            string accesskey = ConfigurationManager.AppSettings["AccessKey"];
            Logger log = new Logger(ConfigurationManager.AppSettings["LoggerDirectory"]);

            int zip;
            Console.Write("Enter Zipcode: ");
            zip = Convert.ToInt32(Console.ReadLine());

            GetWeatherCondition _getWeatherCondition = new GetWeatherCondition(log, apiurl, accesskey, zip);

            _getWeatherCondition.RequestWeatherCondition();
        }
    }
}
