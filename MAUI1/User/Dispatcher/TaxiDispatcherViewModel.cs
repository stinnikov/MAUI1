using CommunityToolkit.Maui.Core.Extensions;
using Mapsui.Providers.Wms;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Maps;
using MAUI1.User.Order;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Dispatcher
{
    public enum OrderFilterType
    {
        Все,
        Ожидает,
        Выполняется
    }
    public class TaxiDispatcherViewModel : UserVM
    {
        public Color DefaultClientColor { get { return Colors.Red; } }
        public Color DefaultSelectedClientColor => Colors.DeepPink;
        public Color DefaultDriverColor { get { return Colors.SkyBlue; } }
        public TaxiDispatcherModel TaxiDispatcher { get; set; }
        private OrderFilterType _orderFilter = OrderFilterType.Все;
        public OrderFilterType OrdersFilter
        {
            get => _orderFilter;
            set
            {
                _orderFilter = value;
                Orders = null;
                OnPropertyChanged(nameof(OrdersFilter));
            }
        }

        public ObservableCollection<OrderViewModel> _orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                if (OrdersFilter == OrderFilterType.Все)
                {
                    _orders = TaxiDispatcher.OrdersCollection;
                }
                else if (OrdersFilter == OrderFilterType.Ожидает)
                {
                    _orders = TaxiDispatcher.OrdersCollection.Where(item => item.Order.Status == OrderStatusType.Waiting).ToObservableCollection();
                }
                else
                {
                    _orders = TaxiDispatcher.OrdersCollection.Where(item => item.Order.Status == OrderStatusType.InProgress).ToObservableCollection();
                }
                OnPropertyChanged(nameof(Orders));
            }
        }

        private bool _isShownAllOrders = true;
        public bool IsShownAllOrders
        {
            get => _isShownAllOrders;
            set
            {
                _isShownAllOrders = value;
                OnPropertyChanged("IsShownAllOrders");
            }
        }
        private bool _isDriversShownOnTheMap = true;
        public bool IsDriversShownOnTheMap
        {
            get => _isDriversShownOnTheMap;
            set
            {
                _isDriversShownOnTheMap = value;
                OnPropertyChanged("IsDriversShownOnTheMap");
            }
        }
        private Pin selectedClientStartingPin;
        private Pin selectedClientEndingPin;
        private DriverViewModel _selectedDriverViewModel;
        public DriverViewModel SelectedDriverVM
        {
            get => _selectedDriverViewModel;
            set
            {
                if (value != null)
                {
                    _selectedDriverViewModel = value;
                    OnPropertyChanged(nameof(SelectedDriverVM));
                }
            }
        }
        private OrderViewModel _selectedOrderVM;
        public OrderViewModel SelectedOrderVM
        {
            get => _selectedOrderVM;
            set
            {
                if (value != null)
                {
                    _selectedOrderVM = value;
                    OnPropertyChanged(nameof(SelectedOrderVM));
                }
            }
        }
        private DispatcherMapController _dispatcherMapController;
        public DispatcherMapController DispatcherMapController
        {
            get => _dispatcherMapController;
            set
            {
                if (value != null)
                {
                    _dispatcherMapController = value;
                    _dispatcherMapController.IsMyLococationNeedable = false;
                }
            }
        }

        public ICommand ShowClientPinCommand { get; set; }
        public ICommand SelectOrderCommand { get; set; }
        public ICommand MoveToOrdersPageCommand { get; set; }
        public ICommand DriversOnTheMapShowCheckedChangedCommand { get; set; }
        public ICommand AllClientOnTheMapCheckedChangedCommand { get; set; }
        public ICommand DriverConfirmationCommand { get; set; }
        public ICommand SelectFilterCommand { get; set; }
        public ICommand test { get; set; }
        public TaxiDispatcherViewModel(TaxiDispatcherModel tdispatcher, MapView mapview)
        {
            User = tdispatcher;
            TaxiDispatcher = tdispatcher;
            DispatcherMapController = new(mapview, this);
            DispatcherVMInit();
        }
        public TaxiDispatcherViewModel(TaxiDispatcherModel tdispatcher)
        {
            User = tdispatcher;
            TaxiDispatcher = tdispatcher;
            _orders = TaxiDispatcher.OrdersCollection;
            DispatcherVMInit();
        }
        private void DispatcherVMInit()
        {
            SelectOrderCommand = new Command(async obj =>
            {
                OrderViewModel order = obj as OrderViewModel;
                var КлиентVM = order.ClientVM;
                var pin = DispatcherMapController.OrderPinDataCollection.FirstOrDefault(item => item.Client == КлиентVM);
                if (pin != null)
                {
                    
                    pin.StartingPointPin.IsVisible = true;
                    pin.EndingPointPin.IsVisible = true;
                    this.DispatcherMapController.mapView.Pins.Add(pin.StartingPointPin);
                    this.DispatcherMapController.mapView.Pins.Add(pin.EndingPointPin);
                    this.DispatcherMapController.CenterMapOnPin(pin.StartingPointPin);
                    SelectedOrderVM = order;
                }
                await Navigation.PopAsync();
            });
            DriversOnTheMapShowCheckedChangedCommand = new Command(() =>
            {
                if (IsDriversShownOnTheMap)
                {
                    foreach (var element in DispatcherMapController.OrderPinDataCollection)
                    {
                        if (element.Client.GetType() != typeof(DriverViewModel))
                            continue;
                        element.StartingPointPin.IsVisible = true;
                        element.EndingPointPin.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var element in DispatcherMapController.OrderPinDataCollection)
                    {
                        if (element.Client.GetType() != typeof(DriverViewModel))
                            continue;
                        element.StartingPointPin.IsVisible = false;
                        element.EndingPointPin.IsVisible = false;
                    }
                }
            });
            DriverConfirmationCommand = new Command(
                async () =>
                {
                    bool accepted = await Application.Current.MainPage.DisplayAlert("", "Потвердить выбор водителя?", "Да", "Нет");
                    if (accepted)
                    {
                        TCPCLient.CreateDriverOrderRequest(SelectedDriverVM.UserPhoneNumber, SelectedOrderVM.ClientPhoneNumber);
                    }
                });
            MoveToOrdersPageCommand = new Command(async () =>
            {
                await Navigation.PushAsync(new OrdersCollectionPage(this));
            });
            SelectFilterCommand = new Command(async () =>
            {
                string action = await Navigation.NavigationStack.LastOrDefault().DisplayActionSheet($"Фильтр заказов (Текущий:{OrdersFilter.ToString()})", "Назад", null, OrderFilterType.Все.ToString(), OrderFilterType.Выполняется.ToString(), OrderFilterType.Ожидает.ToString());
                if (action != "Назад")
                {
                    if (action == "Все")
                    {
                        OrdersFilter = OrderFilterType.Все;
                    }
                    else if (action == "Ожидает")
                    {
                        OrdersFilter = OrderFilterType.Ожидает;
                    }
                    else if (action == "Выполняется")
                    {
                        OrdersFilter = OrderFilterType.Выполняется;
                    }
                }
            });
        }
        public void DispatcherMapViewInit(MapView mapview, List<OrderPinData> pins)
        {
            DispatcherMapController = new(mapview, this);
        }
        public new async void Poll()
        {
           var data = await base.Poll() as object[];
            if (data != null)
            {
                var ordersCollection = (data[0] as List<OrderViewModel>).ToObservableCollection();
                var driversCollection = (data[1] as List<DriverViewModel>).ToObservableCollection();
                if (data != null)
                {
                    if (this.TaxiDispatcher.OrdersCollection.Count != 0)
                    {
                        foreach (var element in ordersCollection)
                        {
                            var order = this.TaxiDispatcher.OrdersCollection.FirstOrDefault(item => item.ClientPhoneNumber == element.ClientPhoneNumber);
                            if (order != null)
                            {
                                order.Order = element.Order;
                            }
                        }
                    }
                    else
                    {
                        this.TaxiDispatcher.OrdersCollection = ordersCollection;
                    }
                    if (this.TaxiDispatcher.Drivers.Count != 0)
                    {
                        foreach (var element in driversCollection)
                        {
                            var driver = this.TaxiDispatcher.Drivers.FirstOrDefault(item => item.UserPhoneNumber == element.UserPhoneNumber);
                            if (driver != null)
                            {
                                driver.Order = element.Order;
                                driver.User.Longitude = element.User.Longitude;
                                driver.User.Latitude = element.User.Latitude;
                            }
                        }
                    }
                    else
                    {
                        this.TaxiDispatcher.Drivers = driversCollection;
                    }
                    System.Timers.Timer timer = new System.Timers.Timer();
                    timer.Elapsed += (o, e) => { timer.Stop(); DispatcherMapController?.SetPins(); this.Poll(); };
                    timer.Interval = 5 * 1000;
                    timer.Start();
                }
            }
        }
        //public async void Poll()
        //{
        //    try
        //    {
        //        object[] data = await TCPCLient.PollServerData(this.User) as object[];
        //        if (data != null)
        //        {
        //            this.TaxiDispatcher.OrdersCollection = (data[0] as List<OrderViewModel>).ToObservableCollection();
        //            this.TaxiDispatcher.Drivers = (data[1] as List<DriverViewModel>).ToObservableCollection();
        //            Poll();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
        //        return;
        //    }
        //}
    }
}
