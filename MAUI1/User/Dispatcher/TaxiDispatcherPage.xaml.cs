using Mapsui.Projections;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using System.Diagnostics;

namespace MAUI1.User.Dispatcher;

public partial class TaxiDispatcherPage : ContentPage
{
	public TaxiDispatcherPage()
	{
		InitializeComponent();
        //var pins = GetPinData();
        var pin1 = new Pin(mapView) //водила
        {
            Position = new Mapsui.UI.Maui.Position( 53.12857, 90.51241),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "Село Аскиз, улица Красноармейская 3",
            Scale = 0.7F,
            Color = dvm.DefaultDriverColor,
        };
        DriverViewModel driver1 = new() { User = new("Karlo", "Driver", "89130581262", "driver1@mail.ru", "dadada", User.UserType.Driver) };
        UserPinData upin1 = new UserPinData(pin1,driver1);
        var pin2 = new Pin(mapView) //водила
        {
            Position = new Mapsui.UI.Maui.Position(53.12519f, 90.52708),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "Республика Хакасия",
            Scale = 0.7F,
            Color = dvm.DefaultDriverColor//TaxiDispatcherViewModel.DefaultDriverColor,
        };
        DriverViewModel driver2 = new() { User = new("Pacik", "Driver", "89130591262", "driver2@mail.ru", "dadada3", User.UserType.Driver) };
        UserPinData upin2 = new UserPinData(pin2, driver2);
        var pin3 = new Pin(mapView) //клиент
        {
            Position = new Mapsui.UI.Maui.Position(53.13029, 90.52082),
            Label = $"DDDDDD",
            Address = "Село Аскиз, улица Базинская 5",
            Scale = 0.7F,
            Color = dvm.DefaultClientColor,
        };
        ClientViewModel client1 = new() { User = new("Karlo", "Client", "89130581362", "client1@mail.ru", "dadada", User.UserType.Client) };
        UserPinData upin3 = new UserPinData(pin3, client1);
        client1.Order = new OrderViewModel(client1, driver1, upin3.Pin.Address, upin1.Pin.Address);
        var pin4 = new Pin(mapView) //клиент
        {
            Position = new Mapsui.UI.Maui.Position(53.13027, 90.5345),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "Республика Хакасия",
            Scale = 0.7F,
            Color = dvm.DefaultClientColor,
        };
        ClientViewModel client2 = new() { User = new("Pacik", "Client", "89140581262", "client2@mail.ru", "dadada3", User.UserType.Client) };
        UserPinData upin4 = new UserPinData(pin4, client2);
        client2.Order = new OrderViewModel(client2, driver2, upin4.Pin.Address, upin2.Pin.Address);
        List<UserPinData> pins = new List<UserPinData>() { upin1, upin2, upin3, upin4 };

        dvm.DispatcherMapController = new DispatcherMapController(mapView, pins);
        dvm.Collection.Add(client1.Order);
        dvm.Collection.Add(client2.Order);
	}

    private List<UserPinData> GetUserPinData()
    {
        //взять данные с сервака и передать в конструктор
        return default;
    }

    private void ClientList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }
}