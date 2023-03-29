namespace MAUI1;

public partial class ClientPage : ContentPage
{
	public ClientPage()
	{
		InitializeComponent();
		BindingContext = new Client.ClientViewModel();
	}
}