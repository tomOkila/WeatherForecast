using CurrentWeather;
using Forecast.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Forecast
{
    public class WeatherFacade
    {
        public async static Task<RootObject> GetWeatherDetails(double lat, double lon)
        {
            var http = new HttpClient();
            var url = String.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid=d7d3346a960174c8076375c099739df7&cnt=5&units=metric", lat, lon);
            var response = await http.GetAsync(url);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));

            var result = (RootObject)serializer.ReadObject(ms);
            return result;
        }


        public async static void PopulateWeatherDetails(ObservableCollection<ForecastList> foreCast)
        {

            var position = await LocationManager.GetPosition();

            var lat = position.Coordinate.Point.Position.Latitude;
            var lon = position.Coordinate.Point.Position.Longitude;


            var weatherDetails = await GetWeatherDetails(lat, lon);
            var weatherResults = weatherDetails.list;
            var ImageResults = weatherResults;
            foreach (var data in ImageResults)
            {
                foreCast.Add(data);
                
            }

           


        }

    }
}
