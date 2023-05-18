using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;

namespace MAUI1.User.Driver;

public partial class DriverPage : ContentPage
{
	public DriverPage(DriverViewModel dvm)
	{
		InitializeComponent();
        dvm.MapController = new MapController(mapView);
        this.BindingContext = dvm;
        //var dvm = new DriverViewModel(mapView);
        //this.BindingContext = dvm;
    }
}