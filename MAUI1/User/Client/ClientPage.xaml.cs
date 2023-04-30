namespace MAUI1.User.Client;

public partial class ClientPage : ContentPage
{
	public ClientPage()
	{
		InitializeComponent();
		BindingContext = new User.Client.ClientViewModel();
	}
}