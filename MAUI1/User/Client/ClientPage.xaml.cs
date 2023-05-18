using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Order;
using OsmSharp;
namespace MAUI1.User.Client;

public partial class ClientPage : ContentPage
{
	public ClientPage()
	{
		
		InitializeComponent();
		ClientModel client = new("Karlo", "Client", "89130581262", "client@mail.ru", "dadada", User.UserType.Client);
		
        ClientViewModel cvm = new(mapView, client);
        OrderViewModel orderViewModel = new OrderViewModel(cvm, "dada", "netnet");
        cvm.Order = orderViewModel;
        this.BindingContext = cvm;
	}

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		if (orderStackLayoutRow.Height.Value == 0)
		{
            orderStackLayoutRow.Height = new GridLength('*');
            orderStackLayout.IsVisible = true;
        }
		else
		{
            orderStackLayoutRow.Height = new GridLength(0);
            orderStackLayout.IsVisible = false;
        }
    }
}