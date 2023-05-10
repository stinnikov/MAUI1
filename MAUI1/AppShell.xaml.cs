namespace MAUI1;

public partial class AppShell : Shell
{
    public AppShell(string acc_type)
	{
		InitializeComponent();
        //if (acc_type == "client")
        //{
        //    Account.Content = new User.Client.ClientPersonalAccountPage();
        //}
        //else if (acc_type == "driver")
        //{
        //    Account.Content = new User.Driver.DriverPage();
        //}
        //IsLogged = true;
	}
    public AppShell()
    {
        InitializeComponent();
    }
}
