using Org.Apache.Http;
using WeatherApp.Services;

namespace WeatherApp;

public partial class WeatherPage : ContentPage
{
	private static double latitude, longitude; //24.1469, 120.6839

    public List<Models.List> WeatherList;

    public WeatherPage()
	{
		InitializeComponent();
		WeatherList = new List<Models.List>();
	}

    protected override async void OnAppearing()
	{
		base.OnAppearing();
		await GetLocation();
		await GetWeatherDataByLocation(latitude, longitude);
	}

	public async Task GetGeoLocation()
	{
		var location = await Geolocation.GetLocationAsync();
		latitude = location.Latitude;
		longitude = location.Longitude;
	}

	public async Task GetWeatherDataByLocation(double lat, double lon)
	{
        var result = await ApiService.GetWeather(latitude, longitude);

        foreach (var item in result.List)
        {
            WeatherList.Add(item);
        }
        CvWeather.ItemsSource = WeatherList;

        lblCity.Text = result.City.Name;
        lblWeatherDescription.Text = result.List[0].Weather[0].Description;
        lblTemperature.Text = result.List[0].Main.temperature + "¢XC";
        lblHumidity.Text = result.List[0].Main.Humidity + "%";
        lblWind.Text = result.List[0].Wind.Speed + "km/h";
        weatherIcon.Source = result.List[0].Weather[0].customIcon;
    }

    public async Task GetWeatherDataByCity(string city)
    {
        var result = await ApiService.GetWeatherByCity(city);
        foreach (var item in result.List)
        {
            WeatherList.Add(item);
        } 
        CvWeather.ItemsSource = WeatherList;
    }

    public async Task GetLocation()
    {
        var location = await Geolocation.GetLocationAsync();
        if (location != null)
        {
            latitude = location.Latitude;
            longitude = location.Longitude;
        }
    }

    public void UpdateUI(dynamic result)
    {
        foreach (var item in result.list)
        {
            WeatherList.Add(item);
        }
    }

    private async void TapLocation(object sender, EventArgs e)
    {
        await GetLocation();
        await GetWeatherDataByLocation(latitude, longitude);
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var response = await DisplayPromptAsync(title:"", message:"", placeholder:"Search weather by City", accept:"Search", cancel:"Cancel");
        if (response != null)
        {
            await GetWeatherDataByCity(response);
        }
    }
}