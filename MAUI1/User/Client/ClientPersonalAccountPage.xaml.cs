using OsmSharp;
namespace MAUI1.User.Client;

public partial class ClientPersonalAccountPage : ContentPage
{
	public ClientPersonalAccountPage()
	{
		
		InitializeComponent();
		MapController mc = new MapController(mapView);
		this.BindingContext = mc;
	}
}