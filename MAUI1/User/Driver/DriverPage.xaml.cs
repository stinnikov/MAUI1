using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;

namespace MAUI1.User.Driver;

public partial class DriverPage : ContentPage
{
	public DriverPage()
	{
		InitializeComponent();
        var dvm = new DriverViewModel();
		dvm.MapController = new MapController(mapView);
        ClientViewModel client1 = new() { Client = new() { FirstName = "Karlo_client", Email = "client1@mail.ru", PhoneNumber = "89130581262" } };
        dvm.Driver = new() { FirstName = "Pacik_Driver" };
        dvm.DriverOrder = new OrderViewModel(client1, dvm, "Село Аскиз улица Базинская 5", "Село Аскиз улица Карланская 25");
        this.BindingContext = dvm;
        //var dvm = new DriverViewModel(mapView);
        //this.BindingContext = dvm;
    }
}