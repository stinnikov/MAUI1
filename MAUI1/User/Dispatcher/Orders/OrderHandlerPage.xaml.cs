using Mapsui.UI.Maui;

namespace MAUI1.User.Dispatcher.Orders;

public partial class OrderHandlerPage : ContentPage
{
	public OrderHandlerPage()
	{
		InitializeComponent();
        var pin1 = new Pin(mapView)
        {
            Position = new Mapsui.UI.Maui.Position(53, 93),
            Type = PinType.Pin,
            Label = $"DDDDDDDDDDDDDDDDDDDD",
            Scale = 0.7F,
            Color = Colors.Red,
        };
        //OrderViewModel ovm = new OrderViewModel(mapView, new List<UserPinData> { new UserPinData() { Pin = pin1, User = new UserModel() { Name = "Shamil" } } }) ;
		//this.BindingContext = ovm;
	}
}