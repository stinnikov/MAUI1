using CommunityToolkit.Maui.Core.Extensions;
using Mapsui.Providers.Wms;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Order;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static TDispatcherContext DispatcherContext { get; set; } = new TDispatcherContext();
        public static List<LocationModel> Locations { get; set; }
        public TaxiDispatcherModel TaxiDispatcher { get; set; }
        public OrderFilterType OrderFilter { get; set; } = OrderFilterType.Все;
        public ObservableCollection<OrderViewModel> Orders 
        {
            get
            {
                if (OrderFilter == OrderFilterType.Все)
                {
                   return TaxiDispatcher.OrdersCollection;
                }
                else if(OrderFilter == OrderFilterType.Ожидает)
                {
                    return TaxiDispatcher.OrdersCollection.Where(item => item.Order.Status == OrderStatusType.Waiting).ToObservableCollection();
                }
                else
                {
                    return TaxiDispatcher.OrdersCollection.Where(item => item.Order.Status == OrderStatusType.InProgress).ToObservableCollection();
                }
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
        private bool _isAllClientsShownOnTheMap = false;
        public bool IsAllClientsShownOnTheMap
        {
            get => _isAllClientsShownOnTheMap;
            set
            {
                _isAllClientsShownOnTheMap = value;
                if(value && SelectedClientVM != null)
                {
                    selectedClientStartingPin.Color = DefaultSelectedClientColor;
                    selectedClientEndingPin.Color = DefaultSelectedClientColor;
                }
                else if(SelectedClientVM != null)
                {
                    selectedClientStartingPin.Color = DefaultClientColor;
                    selectedClientEndingPin.Color = DefaultClientColor;
                }
                OnPropertyChanged("IsAllClientsShownOnTheMap");
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
                if(value != null)
                {
                    _selectedDriverViewModel = value;
                    OnPropertyChanged(nameof(SelectedDriverVM));
                }
            }
        }
        private ClientViewModel _selectedClientVM;
        public ClientViewModel SelectedClientVM 
        { 
            get { return _selectedClientVM; } 
            set 
            { 
                if (value != null) 
                {
                    if (selectedClientStartingPin != null && selectedClientEndingPin != null)
                    {
                        selectedClientStartingPin.Color = DefaultClientColor;
                        selectedClientEndingPin.Color = DefaultClientColor;
                    }
                    _selectedClientVM = value;
                    selectedClientStartingPin = (this.DispatcherMapController?.OrderPinDataCollection?.Where(item => item.Client == SelectedClientVM).FirstOrDefault()?.StartingPointPin);
                    selectedClientEndingPin = (this.DispatcherMapController?.OrderPinDataCollection?.Where(item => item.Client == SelectedClientVM).FirstOrDefault()?.EndingPointPin);
                    if (IsAllClientsShownOnTheMap)
                    {
                        selectedClientStartingPin.Color = DefaultSelectedClientColor;
                        selectedClientEndingPin.Color = DefaultSelectedClientColor;
                    }
                    else
                    {
                        selectedClientStartingPin.Color = DefaultClientColor;
                        selectedClientEndingPin.Color = DefaultClientColor;
                    }
                    SelectedOrderVM = value.Order;
                    OnPropertyChanged(nameof(SelectedClientVM)); 
                }
            } 
        }
        private OrderViewModel _selectedOrderVM;
        public OrderViewModel SelectedOrderVM
        {
            get => _selectedOrderVM;
            set
            {
                if(value != null)
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
                if(value != null)
                {
                    _dispatcherMapController = value;
                }
            }
        }
        
        public ICommand ShowClientPinCommand { get; set; }
        public ICommand ClientOrderSelectedCommand { get; set; }
        public ICommand DriversOnTheMapShowCheckedChangedCommand { get; set; }
        public ICommand AllClientOnTheMapCheckedChangedCommand { get; set; }
        public ICommand DriverConfirmationCommand { get; set; }
        public ICommand test { get; set; }
        public TaxiDispatcherViewModel(TaxiDispatcherModel tdispatcher, MapView mapview, List<OrderPinData> pins)
        {
            User = tdispatcher;
            TaxiDispatcher = tdispatcher;
            DispatcherMapController = new(mapview, pins, this);
            DispatcherVMInit();
        }
        public TaxiDispatcherViewModel(TaxiDispatcherModel tdispatcher)
        {
            User = tdispatcher;
            TaxiDispatcher = tdispatcher;
            DispatcherVMInit();
        }
        private void DispatcherVMInit()
        {
            Locations = DispatcherContext.Locations.Local.ToList();
            ShowClientPinCommand = new Command(obj =>
            {
                ShowClientPin(obj as UserVM);
            });
            ClientOrderSelectedCommand = new Command(obj =>
            {
                var clientVM = (obj as OrderViewModel).ClientVM;
                var pin = ShowClientPin(clientVM);
                IsShownAllOrders = !IsShownAllOrders;
                this.DispatcherMapController.CenterMapOnPin(pin.StartingPointPin);
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
            AllClientOnTheMapCheckedChangedCommand = new Command(() =>
            {
                if(IsAllClientsShownOnTheMap)
                {
                    foreach(var element in DispatcherMapController.OrderPinDataCollection)
                    {
                        if (element.Client.GetType() != typeof(ClientViewModel))
                            continue;
                        element.StartingPointPin.IsVisible = true;
                        element.EndingPointPin.IsVisible = true;
                    }
                }
                else
                {
                    foreach (var element in DispatcherMapController.OrderPinDataCollection)
                    {
                        if (element.Client.GetType() != typeof(ClientViewModel))
                            continue;
                        if (element.Client == SelectedClientVM)
                        {
                            continue;
                        }
                        element.StartingPointPin.IsVisible = false;
                        element.EndingPointPin.IsVisible = false;
                    }
                }
            });
            DriverConfirmationCommand = new Command(
                async() =>
                {
                    bool accepted = await Application.Current.MainPage.DisplayAlert("","Потвердить выбор водителя?", "Да", "Нет");
                    if(accepted)
                    {
                        TCPCLient.CreateDriverOrderRequest(SelectedDriverVM.UserPhoneNumber, SelectedClientVM.UserPhoneNumber);
                    }
                });
            test = new Command(obj =>
            {
                var mapView = obj as MapView;
                //var pins = GetPinData();
                
                DriverViewModel driver = new() { User = new("driver", "driver", "89130581263", "driver@mail.ru", "driver", UserType.Driver) };
                
                
                ClientViewModel client = new() { User = new("client", "client", "89130581262", "client@mail.ru", "client", UserType.Client) };
                OrderModel order1 = new OrderModel("Улица Киренского 26, Красноярск", "Проспект Свободный 76Н", client.User);
                OrderViewModel ovm = new OrderViewModel(order1, client, driver);
                var pin1 = new Pin(mapView) //водила
                {
                    Position = new Mapsui.UI.Maui.Position(53.12857, 90.51241),
                    Type = PinType.Pin,
                    Label = $"Точка отправки заказа водителя",
                    Address = driver.Order.StartingPoint,
                    Scale = 0.7F,
                    Color = this.DefaultDriverColor,
                };
                var pin2 = new Pin(mapView) //водила
                {
                    Position = new Mapsui.UI.Maui.Position(53.12519f, 90.52708),
                    Type = PinType.Pin,
                    Label = $"Точка прибытия заказа водителя",
                    Address = driver.Order.EndingPoint,
                    Scale = 0.7F,
                    Color = this.DefaultDriverColor//TaxiDispatcherViewModel.DefaultDriverColor,
                };
                var pin3 = new Pin(mapView) //клиент
                {
                    Position = new Mapsui.UI.Maui.Position(53.13029, 90.52082),
                    Label = $"Точка отправки заказа клиента",
                    Address = client.Order.StartingPoint,
                    Scale = 0.7F,
                    Color = this.DefaultClientColor,
                };
                var pin4 = new Pin(mapView) //клиент
                {
                    Position = new Mapsui.UI.Maui.Position(53.13027, 90.5345),
                    Type = PinType.Pin,
                    Label = $"Точка прибытия заказа клиента",
                    Address = client.Order.EndingPoint,
                    Scale = 0.7F,
                    Color = this.DefaultClientColor,
                };
                OrderPinData upin1 = new OrderPinData(pin1, pin2, client,driver);
                List<OrderPinData> pins = new List<OrderPinData>() { upin1 };

                this.DispatcherMapController = new DispatcherMapController(mapView, pins, this);
                this.TaxiDispatcher.OrdersCollection.Add(client.Order);
            });
        }
        public void DispatcherMapViewInit(MapView mapview, List<OrderPinData> pins)
        {
            DispatcherMapController = new(mapview, pins, this);
        }
        public async void SetPins()
        {
            List<OrderPinData> userPins = new List<OrderPinData>();
            Pin startingPointPin = default;
            Pin endingPointPin = default;
            foreach(var element in TaxiDispatcher.OrdersCollection)
            {
                double startingPointLongitude;
                double startingPointLatitude;
                var startingPointLocation = Locations.FirstOrDefault(item => item.Address == element.Order.StartingPoint);
                if(startingPointLocation != null)
                {
                    startingPointLongitude = startingPointLocation.Longitude;
                    startingPointLatitude = startingPointLocation.Latitude;
                    startingPointPin = new Pin()
                    {
                        Position = new Mapsui.UI.Maui.Position(startingPointLatitude, startingPointLongitude),
                        Type = PinType.Pin,
                        Label = $"Точка отправки заказа",
                        Address = element.Order.EndingPoint,
                        Scale = 0.7F,
                        Color = this.DefaultClientColor,
                    };
                }
                else
                {
                    var locations = await Geocoding.GetLocationsAsync(element.Order.StartingPoint);
                    var loc = locations.FirstOrDefault();
                    if (loc != null)
                    {
                        startingPointLongitude = loc.Longitude;
                        startingPointLatitude = loc.Latitude;
                        Locations.Add(new LocationModel(element.Order.StartingPoint, startingPointLongitude, startingPointLatitude));
                        startingPointPin = new Pin()
                        {
                            Position = new Mapsui.UI.Maui.Position(startingPointLatitude, startingPointLongitude),
                            Type = PinType.Pin,
                            Label = $"Точка отправки заказа",
                            Address = element.Order.EndingPoint,
                            Scale = 0.7F,
                            Color = this.DefaultClientColor,
                        };
                    }
                }

                double endingPointLongitude;
                double endingPointLatitude;
                var endingPointLocation = Locations.FirstOrDefault(item => item.Address == element.Order.EndingPoint);
                if (endingPointLocation != null)
                {
                    endingPointLongitude = endingPointLocation.Longitude;
                    endingPointLatitude = endingPointLocation.Latitude;
                    endingPointPin = new Pin()
                    {
                        Position = new Mapsui.UI.Maui.Position(endingPointLatitude, endingPointLongitude),
                        Type = PinType.Pin,
                        Label = $"Точка прибытия заказа",
                        Address = element.Order.EndingPoint,
                        Scale = 0.7F,
                        Color = this.DefaultClientColor,
                    };
                }
                else
                {
                    var locations = await Geocoding.GetLocationsAsync(element.Order.EndingPoint);
                    var loc = locations.FirstOrDefault();
                    if (loc != null)
                    {
                        endingPointLongitude = loc.Longitude;
                        endingPointLatitude = loc.Latitude;
                        Locations.Add(new LocationModel(element.Order.StartingPoint, endingPointLongitude, endingPointLatitude));
                        endingPointPin = new Pin()
                        {
                            Position = new Mapsui.UI.Maui.Position(endingPointLatitude, endingPointLongitude),
                            Type = PinType.Pin,
                            Label = $"Точка отправки заказа",
                            Address = element.Order.EndingPoint,
                            Scale = 0.7F,
                            Color = this.DefaultClientColor,
                        };
                    }
                }
                if (startingPointPin != default && endingPointPin != default)
                {
                    if (element.DriverVM == null)
                    {
                        userPins.Add(new OrderPinData(startingPointPin, endingPointPin, element.ClientVM));
                    }
                    else
                    {
                        userPins.Add(new OrderPinData(startingPointPin, endingPointPin, element.ClientVM, element.DriverVM));
                    }
                }
            }
            if (userPins.Count != 0)
            {
                this.DispatcherMapController?.SetPins(userPins);
            }
        }
        private OrderPinData ShowClientPin(UserVM userVM)
        {
            if (SelectedClientVM != null)
            {
                var previousClientPin = DispatcherMapController.OrderPinDataCollection.Where(item => item.Client == SelectedClientVM).FirstOrDefault();
                previousClientPin.StartingPointPin.IsVisible = false;
                previousClientPin.EndingPointPin.IsVisible = false;
                previousClientPin.StartingPointPin.Color = DefaultClientColor;
                previousClientPin.EndingPointPin.Color = DefaultClientColor;
            }
            SelectedClientVM = (ClientViewModel)userVM;
            var clientPin = DispatcherMapController.OrderPinDataCollection.Where(item => item.Client == SelectedClientVM).FirstOrDefault();
            clientPin.StartingPointPin.IsVisible = true;
            clientPin.EndingPointPin.IsVisible = true;
            return clientPin;

        }
        
    }
}
