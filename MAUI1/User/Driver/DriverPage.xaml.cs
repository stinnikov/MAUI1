using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Login;

namespace MAUI1.User.Driver;

public partial class DriverPage : ContentPage
{
	public DriverPage(DriverViewModel dvm)
	{
		InitializeComponent();
        dvm.DriverMapController = new DriverMapController(mapView);
        dvm.Navigation = this.Navigation;
        this.BindingContext = dvm;
        dvm.Poll();
        
        //var dvm = new DriverViewModel(mapView);
        //this.BindingContext = dvm;
    }
    private async void LogoutClicked(object sender, EventArgs e)
    {
        var accepted = await App.Current.MainPage.DisplayAlert("Выход из аккаунта", "Вы уверены?", "Да", "Нет");
        if (accepted)
        {
            if (File.Exists(TCPCLient.accessTokenPath))
            {
                File.Delete(TCPCLient.accessTokenPath);
                var loginPage = new LoginPage();
                this.Navigation.InsertPageBefore(loginPage, this);
                await this.Navigation.PopAsync();
                this.Navigation.RemovePage(this);
            }
        }
    }

    private async void Test(object sender, TappedEventArgs e)
    {
        var accepted = await App.Current.MainPage.DisplayAlert("Подтверждение заказа", "Подтвердите заказ", "Да", "Нет");
    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var ovm = (this.BindingContext as DriverViewModel).Order;
        if (ovm != null)
        {
            await this.Navigation.PushAsync(new DriverOrderDetailsPage(ovm));
        }
    }
}