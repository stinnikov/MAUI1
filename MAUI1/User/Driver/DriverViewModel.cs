using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mapsui.UI.Maui;

namespace MAUI1.User.Driver
{
    internal class DriverViewModel : UserVM
    {
        private MapController MapController { get; set; }
        public DriverModel Driver { get; set; } = new DriverModel();
        public ICommand camanda { get; set; }
        public ICommand AvatarClicked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DriverViewModel(MapView mapview)
        {
            MapController = new MapController(mapview);
            camanda = new Command(() => 
            {
                (Application.Current.MainPage as NavigationPage).PushAsync(new UserAccountPage());
            });
        }
        public DriverViewModel()
        {
            camanda = new Command(() =>
            {
                (Application.Current.MainPage as NavigationPage).PushAsync(new UserAccountPage());
            });
        }
    }
}

