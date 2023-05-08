using Mapsui.Projections;
using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher.Orders
{
    class UserPinData 
    {
        public Pin Pin { get; set; }
        public UserVM User { get; set; }
        public UserPinData(Pin pin, UserVM user)
        {
            Pin = pin;
            User = user;
        }
        public UserPinData()
        {

        }
    }
    internal class OrderMapController : MapController
    {
        public UserVM SelectedPinUser { get; set; }
        public ObservableCollection<UserPinData> PinDataCollection { get; set; } = new();
        public OrderMapController(MapView mapview, List<UserPinData> pins) : base(mapview)
        {
            foreach(var pin in pins)
            {
                if (pin.User.GetType() == typeof(Client.ClientViewModel))
                {
                    pin.Pin.IsVisible = false;
                }
                PinDataCollection.Add(pin);
                this.mapView.Pins.Add(pin.Pin);
            }
            mapView.RefreshGraphics();
            this.mapView.PinClicked += OrderMapView_PinClicked;
        }

        private void OrderMapView_PinClicked(object sender, PinClickedEventArgs e)
        {
            var smc = SphericalMercator.FromLonLat(e.Pin.Position.Longitude, e.Pin.Position.Latitude);
            mapView.Map.Navigator.CenterOnAndZoomTo(new Mapsui.MPoint(smc.x,smc.y), mapView.Map.Navigator.Resolutions[16]);
            SelectedPinUser = PinDataCollection.Where(item => item.Pin == e.Pin).FirstOrDefault()?.User;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
