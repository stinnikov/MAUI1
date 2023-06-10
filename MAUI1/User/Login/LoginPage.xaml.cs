namespace MAUI1.User.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		LoginViewModel lvm = new();
		this.BindingContext = lvm;
		lvm.Navigation = this.Navigation;
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await this.Navigation.PushAsync(new User.Registration.UserRegistrationPage());
    }
    //private void VerticalStackLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    (scrollView as IView)?.InvalidateMeasure(); // if not done, everything else will fail 
    //}
}