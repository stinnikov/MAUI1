using Mapsui.Projections;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Login;
using MAUI1.User.Order;
using System.Diagnostics;

namespace MAUI1.User.Dispatcher;

public partial class TaxiDispatcherPage : ContentPage
{
	public TaxiDispatcherPage(TaxiDispatcherViewModel dvm)
	{
		InitializeComponent();
        this.BindingContext = dvm;
        dvm.DispatcherMapController = new DispatcherMapController(this.mapView, dvm);
        dvm.Navigation = this.Navigation;
        dvm.Poll();
    }
    private List<OrderPinData> GetUserPinData()
    {
        //взять данные с сервака и передать в конструктор
        return default;
    }

    private async void LogoutClicked(object sender, EventArgs e)
    {
        var accepted = await App.Current.MainPage.DisplayAlert("Выход из аккаунта", "Вы уверены?", "Да", "Нет");
        if (accepted)
        {
            if (File.Exists(TCPCLient.accessTokenPath))
            {
                File.Delete(TCPCLient.accessTokenPath);
                var currentPage = this;
                var loginPage = new LoginPage();
                this.Navigation.InsertPageBefore(loginPage, this);
                await this.Navigation.PopAsync();
                this.Navigation.RemovePage(this);
            }
        }
    }
    private async void Test(object sender, TappedEventArgs e)
    {
        var dvm = this.BindingContext as TaxiDispatcherViewModel;
        var d = dvm.TaxiDispatcher.Drivers.First();
        await this.DisplayActionSheet("Выбор водителя", "Назад", "Да", $"Имя водителя: {d.UserFullName}", $"Телефон: {d.UserPhoneNumber}");  
    }
    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
       await this.Navigation.PushAsync(new OrdersCollectionPage(this.BindingContext as TaxiDispatcherViewModel));
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await this.Navigation.PushAsync(new OrdersCollectionPage(this.BindingContext as TaxiDispatcherViewModel));
    }
}