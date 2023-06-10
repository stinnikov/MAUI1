namespace MAUI1.User.Dispatcher;

public partial class OrdersCollectionPage : ContentPage
{
	public OrdersCollectionPage(TaxiDispatcherViewModel dvm)
	{
		InitializeComponent();
		//OrdersCollectionViewModel ovm = new OrdersCollectionViewModel();
		//ovm.TaxiDispatcherViewModel = dvm;
		this.BindingContext = dvm;
	}
}