using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Dispatcher
{
    internal class TaxiDispatcherViewModel : IUserViewModel 
    {
        public ClientViewModel SelectedClientVM { get; set; }
        public OrderMapController OrderMapController { get; set; }
        public ObservableCollection<ClientViewModel> Collection { get; private set; }
        public ICommand AvatarClicked { get; set; } = new Command(() => { });
        public ICommand ShowClientPin { get; set; }

        public TaxiDispatcherViewModel(MapView mapview, List<UserPinData> pins)
        {
            OrderMapController = new(mapview, pins);
            Collection = new ObservableCollection<ClientViewModel>();
            ShowClientPin = new Command(obj =>
            {
                if(SelectedClientVM != null)
                {
                    var previousClientPin = OrderMapController.PinDataCollection.Where(item => item.User == SelectedClientVM).FirstOrDefault();
                    previousClientPin.Pin.IsVisible = false;
                }
                SelectedClientVM = (obj as ClientViewModel);
                var clientPin = OrderMapController.PinDataCollection.Where(item => item.User == SelectedClientVM).FirstOrDefault();
                clientPin.Pin.IsVisible = true;
            });
            
        }
    }
}
