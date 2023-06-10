using MAUI1.User.Order;

namespace MAUI1.User.Driver;

public partial class DriverOrderDetailsPage : ContentPage
{
	public DriverOrderDetailsPage(OrderViewModel ovm)
	{
		InitializeComponent();
		this.BindingContext = ovm;
	}
}