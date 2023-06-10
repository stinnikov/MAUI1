using Mapsui.Projections;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Driver;
using MAUI1.User.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher.Orders
{
    public class OrderPinData 
    {
        public Pin StartingPointPin { get; set; }
        public Pin EndingPointPin { get; set; }
        public ClientViewModel Client { get; set; }
        public DriverViewModel Driver { get; set; }
        public OrderPinData(Pin startingPointPin, Pin endingPointPin, ClientViewModel client)
        {
            StartingPointPin = startingPointPin;
            EndingPointPin = endingPointPin;
            Client = client;
        }
        public OrderPinData(Pin startingPointPin, Pin endingPointPin, ClientViewModel client, DriverViewModel driver)
        {
            StartingPointPin = startingPointPin;
            EndingPointPin = endingPointPin;
            Client = client;
            Driver = driver;
        }
        public OrderPinData()
        {

        }
    }
    public class DriverPinData
    {
        public DriverViewModel Driver { get; set; }
        public Pin PreviousPin { get; set; }
        public Pin _currentPin;
        public Pin CurrentPin
        {
            get => _currentPin;
            set
            {
                if (_currentPin != null)
                {
                    PreviousPin = _currentPin;
                }
                _currentPin = value;
            }
        }
        public DriverPinData(DriverViewModel driver, Pin pin)
        {
            Driver = driver;
            CurrentPin = pin;
        }
    }
    public class DispatcherMapController : MapController
    {
        public TaxiDispatcherViewModel DispatcherVM { get; set; }
        public OrderPinData SelectedOrderPinData { get; set; }
        public ObservableCollection<DriverPinData> IdleDriversPins { get; set; } = new();
        public ObservableCollection<OrderPinData> OrderPinDataCollection { get; set; } = new();
        public DispatcherMapController(MapView mapview, TaxiDispatcherViewModel dispatcherVM) : base(mapview)
        {
            DispatcherVM = dispatcherVM;

            IsMapClickable = false;
            
            this.mapView.PinClicked += MapView_PinClicked;
        }
        public async Task SetPins()
        {
            if (DispatcherVM.Orders.Count != 0)
            {
                if (Locations.Count != 0)
                {
                    foreach (var element in DispatcherVM.TaxiDispatcher.OrdersCollection)
                    {
                        OrderPinData orderPinData;
                        Pin startingPointPin;
                        Pin endingPointPin;
                        var startingPointLocation = Locations.FirstOrDefault(item => item.Latitude == element.Order.StartingPointLatitude && item.Longitude == element.Order.StartingPointLongitude);
                        if (startingPointLocation != null)
                        {
                            startingPointPin = new()
                            {
                                Address = startingPointLocation.Address,
                                Label = "Точка отправки",
                                Position = new Position(element.Order.StartingPointLatitude, element.Order.StartingPointLatitude),
                                Scale = App.DefaultPinScale,
                                Color = App.DefaultOrderColor,
                                Type = PinType.Pin,
                            };
                        }
                        else
                        {
                            var address = await GetAddressFromLonLat(element.Order.StartingPointLongitude, element.Order.StartingPointLatitude);
                            startingPointPin = new()
                            {
                                Address = address,
                                Label = "Точка отправки",
                                Position = new Position(element.Order.StartingPointLatitude, element.Order.StartingPointLatitude),
                                Scale = App.DefaultPinScale,
                                Color = App.DefaultOrderColor,
                                Type = PinType.Pin,
                            };
                        }
                        var endingPointLocation = Locations.FirstOrDefault(item => item.Latitude == element.Order.EndingPointLatitude && item.Longitude == element.Order.EndingPointLongitude);
                        if (endingPointLocation != null)
                        {
                            endingPointPin = new()
                            {
                                Address = endingPointLocation.Address,
                                Label = "Точка прибытия",
                                Position = new Position(element.Order.EndingPointLatitude, element.Order.EndingPointLatitude),
                                Scale = App.DefaultPinScale,
                                Color = App.DefaultOrderColor,
                                Type = PinType.Pin,
                            };
                        }
                        else
                        {
                            var address = await GetAddressFromLonLat(element.Order.EndingPointLongitude, element.Order.EndingPointLatitude);
                            endingPointPin = new()
                            {
                                Address = address,
                                Label = "Точка прибытия",
                                Position = new Position(element.Order.EndingPointLatitude, element.Order.EndingPointLatitude),
                                Scale = App.DefaultPinScale,
                                Color = App.DefaultOrderColor,
                                Type = PinType.Pin,
                            };
                        }
                        if (element.DriverVM == null)
                        {
                            orderPinData = new OrderPinData(startingPointPin, endingPointPin, element.ClientVM);
                        }
                        else
                        {
                            orderPinData = new OrderPinData(startingPointPin, endingPointPin, element.ClientVM, element.DriverVM);
                        }
                        OrderPinDataCollection.Add(orderPinData);
                    }
                }
                else
                {
                    foreach (var element in DispatcherVM.TaxiDispatcher.OrdersCollection)
                    {
                        OrderPinData orderPinData;
                        Pin startingPointPin;
                        Pin endingPointPin;
                        var startingPointAddress = await GetAddressFromLonLat(element.Order.StartingPointLongitude, element.Order.StartingPointLatitude);
                        startingPointPin = new()
                        {
                            Address = startingPointAddress,
                            Label = "Точка отправки",
                            Position = new Position(element.Order.StartingPointLatitude, element.Order.StartingPointLatitude),
                            Scale = App.DefaultPinScale,
                            Color = App.DefaultOrderColor,
                            Type = PinType.Pin,
                        };

                        var endingPointAddress = await GetAddressFromLonLat(element.Order.EndingPointLongitude, element.Order.EndingPointLatitude);
                        endingPointPin = new()
                        {
                            Address = endingPointAddress,
                            Label = "Точка прибытия",
                            Position = new Position(element.Order.EndingPointLatitude, element.Order.EndingPointLatitude),
                            Scale = App.DefaultPinScale,
                            Color = App.DefaultOrderColor,
                            Type = PinType.Pin,
                        };

                        if (element.DriverVM == null)
                        {
                            orderPinData = new OrderPinData(startingPointPin, endingPointPin, element.ClientVM);
                        }
                        else
                        {
                            orderPinData = new OrderPinData(startingPointPin, endingPointPin, element.ClientVM, element.DriverVM);
                        }
                        OrderPinDataCollection.Add(orderPinData);
                    }
                }

                if (DispatcherVM.TaxiDispatcher.Drivers.Count != 0)
                {
                    if (Locations.Count != 0)
                    {
                        foreach (var element in DispatcherVM.TaxiDispatcher.Drivers)
                        {
                            if (element.Order != null)
                            {
                                continue;
                            }
                            if (element.User.Longitude != null && element.User.Latitude != null)
                            {
                                var location = Locations.FirstOrDefault(item => item.Longitude == element.User.Longitude && item.Latitude == element.User.Latitude);
                                if (location != null)
                                {
                                    string address = location.Address;
                                    var driverPin = new Pin()
                                    {
                                        Address = address,
                                        Position = new Position((double)element.User.Latitude, (double)element.User.Longitude),
                                        Label = element.User.PhoneNumber,
                                        Color = App.DefaultDriverColor,
                                        Scale = App.DefaultPinScale,
                                        Type = PinType.Pin,
                                    };
                                    if (!IdleDriversPins.Any(item => item.CurrentPin == driverPin))
                                    {
                                        IdleDriversPins.Add(new(element, driverPin));
                                    }
                                }
                                else
                                {
                                    string address = await GetAddressFromLonLat((double)element.User.Longitude, (double)element.User.Latitude);
                                    var driverPin = new Pin()
                                    {
                                        Address = address,
                                        Position = new Position(location.Latitude, location.Longitude),
                                        Label = element.User.PhoneNumber,
                                        Color = App.DefaultDriverColor,
                                        Scale = App.DefaultPinScale,
                                        Type = PinType.Pin,
                                    };
                                    if (!IdleDriversPins.Any(item => item.CurrentPin == driverPin))
                                    {
                                        IdleDriversPins.Add(new(element, driverPin));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var element in DispatcherVM.TaxiDispatcher.Drivers)
                        {
                            if (element.Order != null)
                            {
                                continue;
                            }
                            if (element.User.Longitude != null && element.User.Latitude != null)
                            {
                                string address = await GetAddressFromLonLat((double)element.User.Longitude, (double)element.User.Latitude);
                                var driverPin = new Pin()
                                {
                                    Address = address,
                                    Position = new Position((double)element.User.Latitude, (double)element.User.Longitude),
                                    Label = element.User.PhoneNumber,
                                    Color = App.DefaultDriverColor,
                                    Scale = App.DefaultPinScale,
                                    Type = PinType.Pin,
                                };
                                if (!IdleDriversPins.Any(item => item.CurrentPin == driverPin))
                                {
                                    IdleDriversPins.Add(new(element, driverPin));
                                }
                            }
                        }
                    }
                    foreach (var element in IdleDriversPins)
                    {
                        if (!mapView.Pins.Any(item => item == element.CurrentPin))
                        {
                            mapView.Pins.Add(element.CurrentPin);
                            if (element.PreviousPin != null)
                            {
                                mapView.Pins.Remove(element.PreviousPin);
                            }
                        }
                    }
                    mapView.RefreshGraphics();
                }
            }
        }
        public void CenterMapOnPin(Pin pin)
        {
            var smc = SphericalMercator.FromLonLat(pin.Position.Longitude, pin.Position.Latitude);
            mapView.Map.Navigator.CenterOn(new Mapsui.MPoint(smc.x, smc.y));
        }
        private new async void MapView_PinClicked(object sender, PinClickedEventArgs e)
        {
            var smc = SphericalMercator.FromLonLat(e.Pin.Position.Longitude, e.Pin.Position.Latitude);
            mapView.Map.Navigator.CenterOn(new Mapsui.MPoint(smc.x, smc.y));
            if (e.Pin.Color == App.DefaultDriverColor)
            {
                var accepted = await App.Current.MainPage.DisplayAlert("Выбор водителя", "Вы уверены в выборе водителя?", "Да", "Нет");
                if (accepted)
                {
                    if (DispatcherVM.SelectedOrderVM != null)
                    {
                        //TODO:отправить водилу на сервак
                        var driverPinData = IdleDriversPins.FirstOrDefault(item => item.CurrentPin == e.Pin);
                        if (driverPinData != null)
                        {
                            var driver = driverPinData.Driver;
                            if (driver != null)
                            {
                                DispatcherVM.SelectedDriverVM = driver;
                                TCPCLient.CreateDriverOrderRequest(driver.User.PhoneNumber, DispatcherVM.SelectedOrderVM.DriverPhoneNumber);
                            }
                        }
                    }
                }
            }
            else
            {
                SelectedOrderPinData = OrderPinDataCollection.Where(item => item.StartingPointPin == e.Pin || item.EndingPointPin == e.Pin).FirstOrDefault();
                if (SelectedOrderPinData != null)
                {
                    if (SelectedOrderPinData.Driver != null)
                    {
                        DispatcherVM.SelectedDriverVM = SelectedOrderPinData.Driver;
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}



