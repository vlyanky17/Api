using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;





namespace ApiTests
{
    public class ConsolidatedWeather
    {
        public object id { get; set; }
        public string weather_state_name { get; set; }
        public string weather_state_abbr { get; set; }
        public string wind_direction_compass { get; set; }
        public DateTime created { get; set; }
        public string applicable_date { get; set; }
        public double min_temp { get; set; }
        public double max_temp { get; set; }
        public double the_temp { get; set; }
        public double wind_speed { get; set; }
        public double wind_direction { get; set; }
        public double air_pressure { get; set; }
        public int humidity { get; set; }
        public double visibility { get; set; }
        public int predictability { get; set; }
    }

    public class Parent
    {
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
    }

    public class Source
    {
        public string title { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public int crawl_rate { get; set; }
    }

    public class Root
    {
        public List<ConsolidatedWeather> consolidated_weather { get; set; }
        public DateTime time { get; set; }
        public DateTime sun_rise { get; set; }
        public DateTime sun_set { get; set; }
        public string timezone_name { get; set; }
        public Parent parent { get; set; }
        public List<Source> sources { get; set; }
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
        public string timezone { get; set; }
    }
    public class CityReqest
    {
        public string title { get; set; }
        public string location_type { get; set; }
        public int woeid { get; set; }
        public string latt_long { get; set; }
    }

    public class DayWeather
    {
        public object id { get; set; }
        public string weather_state_name { get; set; }
        public string weather_state_abbr { get; set; }
        public string wind_direction_compass { get; set; }
        public DateTime created { get; set; }
        public string applicable_date { get; set; }
        public double min_temp { get; set; }
        public double max_temp { get; set; }
        public double the_temp { get; set; }
        public double wind_speed { get; set; }
        public double wind_direction { get; set; }
        public double air_pressure { get; set; }
        public int humidity { get; set; }
        public double? visibility { get; set; }
        public int predictability { get; set; }
    }

    public class Tests
    {   

        [Test]
        public void Test1()
        {
            HttpClient client = new HttpClient();
            string getUrl = "https://www.metaweather.com/api/location/834463/";
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage HttpResponseMessage = httpResponse.Result;
            HttpContent responseContent = HttpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Root jsonRootObject = JsonConvert.DeserializeObject<Root>(data);
          
                for (int i = 0; i < jsonRootObject.consolidated_weather.Count-1; i++)
                {
                    Console.WriteLine("минимальная температура "+ jsonRootObject.consolidated_weather[i].min_temp.ToString());
                 
                }                       
            
            client.Dispose();
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            string latt_long = "53.90255,27.563101";
            HttpClient client = new HttpClient();
            string getUrl = "https://www.metaweather.com/api/location/search/?query=Minsk";
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage HttpResponseMessage = httpResponse.Result;
            HttpContent responseContent = HttpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            List<CityReqest> CityReqestObject = JsonConvert.DeserializeObject<List<CityReqest>>(data);
            client.Dispose();
            if (CityReqestObject[0].latt_long == latt_long)
            {
                Assert.Pass();
            }
            else Assert.Fail();
        }


        [Test]
        public void Test3()
        {
            HttpClient client = new HttpClient();
            string getUrl = "https://www.metaweather.com/api/location/search/?query=Minsk";
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage HttpResponseMessage = httpResponse.Result;
            HttpContent responseContent = HttpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            List<CityReqest> CityReqestObject = JsonConvert.DeserializeObject<List<CityReqest>>(data);
            client.Dispose();
            HttpClient Newclient = new HttpClient();
            getUrl = "https://www.metaweather.com/api/location/" + CityReqestObject[0].woeid.ToString();
            Uri getNewUri = new Uri(getUrl);
            httpResponse = Newclient.GetAsync(getNewUri);
             HttpResponseMessage = httpResponse.Result;
             responseContent = HttpResponseMessage.Content;
             responseData = responseContent.ReadAsStringAsync();
             data = responseData.Result;
            Root jsonRootObject = JsonConvert.DeserializeObject<Root>(data);
            Newclient.Dispose();          
                Console.WriteLine("погода " + jsonRootObject.consolidated_weather[0].weather_state_name);
                Console.WriteLine("ветер " + jsonRootObject.consolidated_weather[0].wind_speed);
                Console.WriteLine("минимальная температура " + jsonRootObject.consolidated_weather[0].min_temp);
                Console.WriteLine("максимальная температура " + jsonRootObject.consolidated_weather[0].max_temp);         
            Assert.Pass();
        }

        [Test]
        public void Test4()
        {
            HttpClient client = new HttpClient();
            string getUrl = "https://www.metaweather.com/api/location/834463/";
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage HttpResponseMessage = httpResponse.Result;
            HttpContent responseContent = HttpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            Root jsonRootObject = JsonConvert.DeserializeObject<Root>(data);
            bool temperatureAnomaly = false;
            client.Dispose();
            for (int i = 0; i < jsonRootObject.consolidated_weather.Count - 1; i++)
                {
                if ((jsonRootObject.consolidated_weather[i].min_temp<-5)|| (jsonRootObject.consolidated_weather[i].min_temp > 25))
                {
                    temperatureAnomaly = true;
                }
                }
            if (temperatureAnomaly == false)
            {
                Assert.Pass();
            }
            else Assert.Fail();
       }

        [Test]
        public void Test5()
        {
            DateTime DayNow = DateTime.Now;
            string Day = DayNow.ToShortDateString();
            string[] strs = Day.Split('.');
            Day = "";
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                Day = Day + strs[i] + '/';
            }
            HttpClient client = new HttpClient();
            string getUrl = "https://www.metaweather.com/api/location/834463/"+ Day;
            Uri getUri = new Uri(getUrl);
            Task<HttpResponseMessage> httpResponse = client.GetAsync(getUri);
            HttpResponseMessage HttpResponseMessage = httpResponse.Result;
            HttpContent responseContent = HttpResponseMessage.Content;
            Task<string> responseData = responseContent.ReadAsStringAsync();
            string data = responseData.Result;
            List<DayWeather> DayWeatherNow = JsonConvert.DeserializeObject<List<DayWeather>>(data);
            client.Dispose();
            DayNow.AddYears(-4);
            Day = DayNow.ToShortDateString();
             strs = Day.Split('.');
            Day = "";
            for (int i = strs.Length - 1; i >= 0; i--)
            {
                Day = Day + strs[i] + '/';
            }
            HttpClient NewClient = new HttpClient();
             getUrl = "https://www.metaweather.com/api/location/834463/" + Day;
             getUri = new Uri(getUrl);
            httpResponse = NewClient.GetAsync(getUri);
             HttpResponseMessage = httpResponse.Result;
             responseContent = HttpResponseMessage.Content;
             responseData = responseContent.ReadAsStringAsync();
             data = responseData.Result;
            List<DayWeather> DayWeatherAgo = JsonConvert.DeserializeObject<List<DayWeather>>(data);
            NewClient.Dispose();
      
            for (int i = 0; i < DayWeatherNow.Count-1; i++)
            {
                if (DayWeatherNow[i].weather_state_name== DayWeatherAgo[i].weather_state_name)
                {
                    Assert.Pass();
                }
            }

            Assert.Fail();
        }

    }
}