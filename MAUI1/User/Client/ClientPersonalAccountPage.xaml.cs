using OsmSharp;
namespace MAUI1.User.Client;

public partial class ClientPersonalAccountPage : ContentPage
{
	public ClientPersonalAccountPage()
	{
		
		InitializeComponent();
		ClientViewModel cvm = new(mapView);
		this.BindingContext = cvm;
	}
}