using Mapsui.Projections;
using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Driver;
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
    public class DispatcherMapController : MapController
    {
        public TaxiDispatcherViewModel DispatcherVM { get; set; }
        public OrderPinData SelectedOrderPinData { get; set; }
        public List<Pin> IdleDriversPins { get; set; } = new();
        public ObservableCollection<OrderPinData> OrderPinDataCollection { get; set; } = new();
        public DispatcherMapController(MapView mapview, List<OrderPinData> pins, TaxiDispatcherViewModel dispatcherVM) : base(mapview)
        {
            DispatcherVM = dispatcherVM;

            IsMapClickable = false;
            if (pins != null)
            {
                foreach (var pin in pins)
                {
                    if (pin.Client.GetType() == typeof(Client.ClientViewModel))
                    {
                        pin.StartingPointPin.IsVisible = false;
                        pin.EndingPointPin.IsVisible = false;
                    }
                    OrderPinDataCollection.Add(pin);
                    this.mapView.Pins.Add(pin.StartingPointPin);
                    this.mapView.Pins.Add(pin.EndingPointPin);
                }
            }
            mapView.RefreshGraphics();
            this.mapView.PinClicked += MapView_PinClicked;
        }
        public DispatcherMapController(MapView mapview, TaxiDispatcherViewModel dispatcherVM) : base(mapview)
        {
            DispatcherVM = dispatcherVM;

            IsMapClickable = false;
            
            this.mapView.PinClicked += MapView_PinClicked;
        }
        public void SetPins(List<OrderPinData> pins)
        {
            if (pins != null)
            {
                foreach (var pin in pins)
                {
                    if (pin.Client.GetType() == typeof(Client.ClientViewModel))
                    {
                        pin.StartingPointPin.IsVisible = false;
                        pin.EndingPointPin.IsVisible = false;
                    }
                    OrderPinDataCollection.Add(pin);
                    this.mapView.Pins.Add(pin.StartingPointPin);
                    this.mapView.Pins.Add(pin.EndingPointPin);
                }
            }
            if(DispatcherVM.Orders.Count != 0)
            {
                //TODO:сделать норм тему
            }
            mapView.RefreshGraphics();
        }
        public void CenterMapOnPin(Pin pin)
        {
            var smc = SphericalMercator.FromLonLat(pin.Position.Longitude, pin.Position.Latitude);
            mapView.Map.Navigator.CenterOn(new Mapsui.MPoint(smc.x, smc.y));
        }
        private new void MapView_PinClicked(object sender, PinClickedEventArgs e)
        {
            var smc = SphericalMercator.FromLonLat(e.Pin.Position.Longitude, e.Pin.Position.Latitude);
            mapView.Map.Navigator.CenterOnAndZoomTo(new Mapsui.MPoint(smc.x,smc.y), mapView.Map.Navigator.Resolutions[16]);
            SelectedOrderPinData = OrderPinDataCollection.Where(item => item.StartingPointPin == e.Pin || item.EndingPointPin == e.Pin).FirstOrDefault();
            if (SelectedOrderPinData != null)
            {
                if (SelectedOrderPinData.Driver != null)
                {
                    DispatcherVM.SelectedDriverVM = SelectedOrderPinData.Driver;
                    DispatcherVM.SelectedClientVM = SelectedOrderPinData.Client;
                    DispatcherVM.IsShownAllOrders = false;
                }
                else
                {
                    DispatcherVM.SelectedClientVM = SelectedOrderPinData.Client;
                    DispatcherVM.IsShownAllOrders = false;
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

