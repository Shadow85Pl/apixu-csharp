using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIXULib
{
    public interface IRepository
    {
        Task<WeatherModel> GetWeatherData(string key, GetBy getBy, string value, Days ForecastOfDays);

        Task<WeatherModel> GetWeatherDataByLatLong(string key, string latitude, string longitude, Days ForecastOfDays);

        Task<WeatherModel> GetWeatherDataByAutoIP(string key, Days ForecastOfDays);

        Task<WeatherModel> GetWeatherData(string key, GetBy getBy, string value);

        Task<WeatherModel> GetWeatherDataByLatLong(string key, string latitude, string longitude);

        Task<WeatherModel> GetWeatherDataByAutoIP(string key);
    }

    public class Repository : IRepository
    {
        private string APIURL = "http://api.apixu.com/v1";

        #region Get Weather Forecast Data

        public async Task<WeatherModel> GetWeatherData(string key, GetBy getBy, string value, Days ForecastOfDays)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequest(MethodType.Forecast, key, getBy, value, ForecastOfDays));
        }

        public async Task<WeatherModel> GetWeatherDataByLatLong(string key, string latitude, string longitude, Days ForecastOfDays)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequestByLatLong(MethodType.Forecast, key, latitude, longitude, ForecastOfDays));
        }

        public async Task<WeatherModel> GetWeatherDataByAutoIP(string key, Days ForecastOfDays)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequestByAutoIP(MethodType.Forecast, key, ForecastOfDays));
        }

        #endregion Get Weather Forecast Data

        #region Get Weather Current Data

        public async Task<WeatherModel> GetWeatherData(string key, GetBy getBy, string value)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequest(MethodType.Current, key, getBy, value));
        }

        public async Task<WeatherModel> GetWeatherDataByLatLong(string key, string latitude, string longitude)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequestByLatLong(MethodType.Current, key, latitude, longitude));
        }

        public async Task<WeatherModel> GetWeatherDataByAutoIP(string key)
        {
            return await GetData(APIURL + RequestBuilder.PrepareRequestByAutoIP(MethodType.Current, key));
        }

        #endregion Get Weather Current Data

        private async Task<WeatherModel> GetData(string url)
        {
            string urlParameters = "";
            var tmp = string.Empty;
            HttpClient client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
            client.BaseAddress = new Uri(url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("gzip"));

            // List data response.
            HttpResponseMessage response = await client.GetAsync(urlParameters);  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                return await response.Content.ReadAsAsync<WeatherModel>();
            }
            else
            {
                return await Task.FromResult(new WeatherModel());
            }
        }
    }
}