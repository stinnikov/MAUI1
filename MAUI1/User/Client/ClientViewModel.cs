using Mapsui.UI.Maui;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Client
{
    public class ClientViewModel : UserVM
    {
        private bool _isOrderPossibilityAvailable = true;
        public bool IsOrderPossibilityAvailable 
        {
            get => _isOrderPossibilityAvailable;
            set
            {
                _isOrderPossibilityAvailable = value;
                OnPropertyChanged(nameof(IsOrderPossibilityAvailable));
            }
        }
        public MapController MapController { get; set; }
        public OrderViewModel Order { get; set; }              
        public ICommand CreateOrderCommand { get;private set; }
        public ICommand PageCommand { get;private set; }
        public ICommand DataClicked { get;private set; }
        public ClientViewModel(MapView mapView, UserModel client)
        {
            User = client;
            MapController mapController = new MapController(mapView);
        }
        public ClientViewModel(UserModel client)
        {
            User = client;
            CommandsInit();
        }
        public ClientViewModel()
        {
        }
        private void CommandsInit()
        {
            CreateOrderCommand = new Command(() =>
            {
                IsOrderPossibilityAvailable = !IsOrderPossibilityAvailable;
            });
    }
    }
}
