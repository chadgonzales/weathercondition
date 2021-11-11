using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast
{
    class GetWeatherCondition
    {
        Logger _logger;
        string _apiurl;
        string _accesskey;
        int _zipcode;

        public GetWeatherCondition(Logger logger, string apiurl, string accesskey, int zipcode)
        {
            _logger = logger;
            _apiurl = apiurl;
            _accesskey = accesskey;
            _zipcode = zipcode;
        }

       public void RequestWeatherCondition()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_apiurl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetWeather(client).Wait();
        }

        private async Task GetWeather(HttpClient cons)
        {
            try
            {
                using (cons)
                {
                    HttpResponseMessage res = await cons.GetAsync("/current?access_key=" + _accesskey + "&query=" + _zipcode);

                    res.EnsureSuccessStatusCode();
                    if (res.IsSuccessStatusCode)
                    {
                        string weather = await res.Content.ReadAsStringAsync();

                        var jobj = JObject.Parse(weather);
                        var weatherDetails = jobj.SelectToken("current")?.ToString();
                        var locationDetails = jobj.SelectToken("location")?.ToString();

                        if (weatherDetails != "")
                        {
                            var weatherObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Weather>(weatherDetails);
                            var location = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(locationDetails);

                            Console.WriteLine("\n");
                            Console.WriteLine("Address: " + location.name + ", " + location.region + ", " + location.country);
                            Console.WriteLine("\n");

                            Console.WriteLine("Should I go outside? " + (weatherObj.Weather_Descriptions[0].Contains("Rain") ? "NO" : "YES"));
                            Console.WriteLine("Should I wear sunscreen? " + (weatherObj.Uv_Index > 3 ? "YES" : "NO"));
                            Console.WriteLine("Can I fly my kite? " + (!weatherObj.Weather_Descriptions[0].Contains("Rain") && weatherObj.Wind_Speed > 15 ? "YES" : "NO"));
                        }

                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLog(DateTime.Now + "|ERROR|Error on API|" + ex.ToString());
            }
        }
    }
}
