namespace MAUI1;

public partial class AppShell : Shell
{
    public bool IsEblan { get; set; }
    public bool IsLogged
    {
        get => (bool)GetValue(IsLoggedProperty);
        set => SetValue(IsLoggedProperty, value);
    }

    public static readonly BindableProperty IsLoggedProperty =
        BindableProperty.Create("IsLogged", typeof(bool), typeof(AppShell), false, propertyChanged: IsLogged_PropertyChanged);

    private static void IsLogged_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        //handle log in/log out event
        if ((bool)newValue)
        {

        }
       //user just logged in logic
        else
        {

        }
      //user just logged out logic
}
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
