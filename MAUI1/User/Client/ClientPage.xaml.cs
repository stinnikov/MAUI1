using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Order;
using OsmSharp;
namespace MAUI1.User.Client;

public partial class ClientPage : ContentPage
{
	public ClientPage(ClientViewModel clientViewModel)
	{
		InitializeComponent();
        clientViewModel.MapController = new MapController(this.mapView);
        this.BindingContext = clientViewModel;
        
	}
}