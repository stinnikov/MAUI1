using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Login;
using MAUI1.User.Maps;
using MAUI1.User.Order;
using OsmSharp;
namespace MAUI1.User.Client;

public partial class ClientPage : ContentPage
{
	public ClientPage(ClientViewModel clientViewModel)
	{
		InitializeComponent();
        clientViewModel.MapController = new MapController(this.mapView);
        this.BindingContext = clientViewModel;
        clientViewModel.Navigation = this.Navigation;
        clientViewModel.Poll();

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
}