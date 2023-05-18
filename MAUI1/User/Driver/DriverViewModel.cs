using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mapsui.UI.Maui;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Order;

namespace MAUI1.User.Driver
{
    public class DriverViewModel : UserVM
    {
        public MapController MapController { get; set; }
        public OrderViewModel Order { get; set; }
        public ImageSource AvatarSource
        {
            get
            {
                var avatarPath = $"{App.projectPersonalFolderPath}\\avatar.png";
                if (File.Exists(avatarPath))
                {
                    return ImageSource.FromFile(avatarPath);
                }
                else
                {
                    var defaultDriverImagePath = $"{App.projectPersonalFolderPath}\\DefaultDriverImage.png";
                    return ImageSource.FromFile(defaultDriverImagePath);
                }
            }
        }
        public ICommand camanda { get; set; }

        public DriverViewModel(MapView mapview, DriverModel driver)
        {
            MapController = new MapController(mapview);
            User = driver;
            camanda = new Command(() => 
            {
                (Application.Current.MainPage as NavigationPage).PushAsync(new UserAccountPage());
            });
        }
        public DriverViewModel()
        {
        }
    }
}

