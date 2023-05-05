namespace MAUI1.User.Driver;

public partial class DriverPage : ContentPage
{
	public DriverPage()
	{
		InitializeComponent();
		var dvm = new DriverViewModel(mapView);
		this.BindingContext = dvm;
	}
}