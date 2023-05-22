using Mapsui.UI.Maui;

namespace MAUI1.User.Dispatcher.Orders;

public partial class OrderHandlerPage : ContentPage
{
	public OrderHandlerPage()
	{
		InitializeComponent();
        MapController mapController = new MapController(mapView);
		//this.BindingContext = ovm;
        
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		if (entry1.Text != null)
		{
            
            var dada = await MapController.GetLonLatFromAddress(entry1.Text);
			label1.Text = $"{dada.Item1};{dada.Item2}";
            Pin pin = new Pin(mapView)
            {
                Position = new Position(dada.Item2, dada.Item1), 
                Address = entry1.Text,
                Color = Colors.Red,
                Scale = 0.67f,
                Type = PinType.Pin,
                Label = entry1.Text,
                
            };
            pin.Callout.Title = "Пинчик";
            pin.Callout.Subtitle = "Пинчик";
            mapView.Pins.Add(pin);
            mapView.Refresh();
            pin.ShowCallout();
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (entry2.Text != null)
        {
            var reverse_geocode_string_splitted = entry2.Text.Split(";");
            var longitude = double.Parse(reverse_geocode_string_splitted[0]);
            var latitude = double.Parse(reverse_geocode_string_splitted[1]);
            label2.Text = await MapController.GetAddressFromLonLat(longitude,latitude);
            
        }
    }
}