namespace MAUI1.User.Admin;

public partial class AdminPage : ContentPage
{
	public AdminPage(AdminVM adminVM)
	{
		InitializeComponent();
		this.BindingContext = adminVM;
	}
}