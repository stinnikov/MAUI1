using Mapsui.Projections;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Order;
using System.Diagnostics;

namespace MAUI1.User.Dispatcher;

public partial class TaxiDispatcherPage : ContentPage
{
	public TaxiDispatcherPage(TaxiDispatcherViewModel dvm)
	{
		InitializeComponent();
        dvm.DispatcherMapController = new DispatcherMapController(this.mapView, dvm);
        dvm.SetPins();
        this.BindingContext = dvm;
	}
    private List<OrderPinData> GetUserPinData()
    {
        //взять данные с сервака и передать в конструктор
        return default;
    }

    private void ClientList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {

    }
}