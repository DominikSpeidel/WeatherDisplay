﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;

namespace InterflexWeatherForecast
{
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "b4288bd546f907154d09db9535e0a9e1";
        private readonly string requestUrl = "https://api.openweathermap.org/data/2.5/weather";

        public MainWindow()
        {
            InitializeComponent();
            UpdateData("Durchhausen");
        }

        public void UpdateData(string city)
        {
            WeatherMapResponse result = GetWeatherData(city);

            // Console.WriteLine(result);

            string finalImage = "sun.png";
            string currentWeather = result.weather[0].main.ToLower();

            if (currentWeather.Contains("cloud"))
            {
                finalImage = "cloud.png";
            }
            else if (currentWeather.Contains("rain"))
            {
                finalImage = "rain.png";
            }
            else if (currentWeather.Contains("snow"))
            {
                finalImage = "snow.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri($"Images/{finalImage}", UriKind.Relative));

            labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
            labelWeatherDescription.Content = result.weather[0].main;
        }

        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();

            var finalUri = requestUrl + "?q=" + city + "&appid=" + apiKey + "&units=metric";
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;

            string response = httpResponse.Content.ReadAsStringAsync().Result;

            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
            
            return weatherMapResponse;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;

            UpdateData(query);
        }
    }
}
