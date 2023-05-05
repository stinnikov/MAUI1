namespace MAUI1.User.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		LoginViewModel lvm = new LoginViewModel();
		this.BindingContext = lvm;
	}
}