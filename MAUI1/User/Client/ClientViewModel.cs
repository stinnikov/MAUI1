using CommunityToolkit.Maui.Alerts;
using Mapsui.UI.Maui;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Maps;
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
        private string _orderFromText;
        public string OrderFromText 
        {
            get => _orderFromText;
            set
            {
                _orderFromText = value;
                OnPropertyChanged(nameof(OrderFromText));
            }
        }
        private string _orderToText;
        public string OrderToText
        {
            get => _orderToText;
            set
            {
                _orderToText = value;
                OnPropertyChanged(nameof(OrderToText));
            }
        }
        public ICommand CreateOrderCommand { get;private set; }
        public ICommand FromEntryTappedCommand { get; set; }
        public ICommand ToEntryTappedCommand { get; set; }
        public ICommand PageCommand { get;private set; }
        public ICommand DataClicked { get;private set; }
        public ICommand Test { get; set; }
        //public ClientViewModel(MapView mapView, UserModel client)
        //{
        //    User = client;
        //    MapController mapController = new MapController(mapView);
        //}
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
            CreateOrderCommand = new Command(async() =>
            {
                if(OrderFromText != null && OrderToText != null)
                {
                    var startingPointAddress = OrderFromText;
                    var startingLonLat = await MapController.GetLonLatFromAddress(startingPointAddress);
                    var endingPointAddress = OrderToText;
                    var endingPointLonLat = await MapController.GetLonLatFromAddress(endingPointAddress);
                    var order = new OrderModel(startingPointAddress, startingLonLat.Item1, startingLonLat.Item2, endingPointAddress, endingPointLonLat.Item1, endingPointLonLat.Item2, this.User);
                    var resp = await TCPCLient.CreateOrderRequest(order);
                }
                string result = await Application.Current.MainPage.DisplayActionSheet("Подтвердить заказ?", "Назад", "Да", $"Откуда: Улица Киренского 26, Красноярск", "Куда: Проспект Свободный 76Н");
            });
            FromEntryTappedCommand = new Command(async () => 
            {
                string result = await App.Current.MainPage.DisplayPromptAsync("Точка отправки", "Введите адрес", "ОК", "Назад", "Адрес");
                if(result != "Назад")
                {
                    OrderFromText = result;
                }
            });
            ToEntryTappedCommand = new Command(async () =>
            {
                string result = await App.Current.MainPage.DisplayPromptAsync("Точка прибытия", "Введите адрес", "ОК", "Назад", "Адрес");
                if (result != "Назад")
                {
                    OrderFromText = result;
                }
            });
            
        }
        public new async void Poll()
        {
            var data = await base.Poll() as List<object>;
            if (data != null)
            {
                var order = data[0] as OrderModel;
                var driver = data[1] as UserModel;
                if (order != null)
                {
                    if (driver != null)
                    {
                        var driverVM = new DriverViewModel(driver);
                        this.Order.Order = order;
                        this.Order.ClientVM = this;
                        this.Order.DriverVM = driverVM;
                    }
                    else
                    {
                        this.Order.Order = order;
                        this.Order.ClientVM = this;
                    }
                }
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += (o, e) => { timer.Stop(); this.Poll(); };
                timer.Interval = 5 * 1000;
                timer.Start();
            }
        }
    }
}
