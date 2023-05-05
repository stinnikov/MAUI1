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
        var pin1 = new Pin(mapView) //������
        {
            Position = new Mapsui.UI.Maui.Position( 53.12857, 90.51241),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "���������� �������",
            Scale = 0.7F,
            Color = Colors.Yellow,
        };
        DriverViewModel driver1 = new() { Driver = new() { Name = "Karlik_Driver" } };
        UserPinData upin1 = new UserPinData(pin1,driver1);
        var pin2 = new Pin(mapView) //������
        {
            Position = new Mapsui.UI.Maui.Position(53.12519f, 90.52708),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "���������� �������",
            Scale = 0.7F,
            Color = Colors.Yellow,
        };
        DriverViewModel driver2 = new() { Driver = new() {Name = "Pacik_Driver" } };
        UserPinData upin2 = new UserPinData(pin2, driver2);
        var pin3 = new Pin(mapView) //������
        {
            Position = new Mapsui.UI.Maui.Position(53.13029, 90.52082),
            Label = $"DDDDDD",
            Address = "���������� �������",
            Scale = 0.7F,
            Color = Colors.Red,
        };
        ClientViewModel client1 = new() { Client = new() { Name = "Karlo_client" } };
        UserPinData upin3 = new UserPinData(pin3, client1);
        var pin4 = new Pin(mapView) //������
        {
            Position = new Mapsui.UI.Maui.Position(53.13027
, 90.5345),
            Type = PinType.Pin,
            Label = $"DDDDDD",
            Address = "���������� �������",
            Scale = 0.7F,
            Color = Colors.Red,
        };
        
        ClientViewModel client2 = new() { Client = new(){Name = "Pacik_Client"} };
        UserPinData upin4 = new UserPinData(pin4, client2);
        List<UserPinData> pins = new List<UserPinData>() { upin1, upin2, upin3, upin4 };
       
        TaxiDispatcherViewModel dvm = new TaxiDispatcherViewModel(mapView, pins);
        dvm.Collection.Add(client1);
        dvm.Collection.Add(client2);
        this.BindingContext = dvm;
	}

    private List<UserPinData> GetUserPinData()
    {
        //����� ������ � ������� � �������� � �����������
        return default;
    }
}