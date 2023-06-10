using MAUI1.User.Order;

namespace MAUI1.User.Client;

public partial class ClientOrderDetailsPage : ContentPage
{
	public ClientOrderDetailsPage(OrderViewModel ovm)
	{
		InitializeComponent();
		this.BindingContext = ovm;
	}
}