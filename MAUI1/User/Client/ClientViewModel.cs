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
        public ObservableCollection<string> dada { get; set; } = new ObservableCollection<string> { "dada","dada", "dada", "dada" };
        private MapController MapController;
        public OrderViewModel Order { get; set; }
        public ImageSource AvatarSource
        {
           get {
                var avatarPath = $"{App.projectPersonalFolderPath}\\avatar.png";
                if (File.Exists(avatarPath))
                {
                    return ImageSource.FromFile(avatarPath);
                }
                else
                {
                    var defaultClientImagePath = $"{App.projectPersonalFolderPath}\\DefaultClientImage.png";
                    return ImageSource.FromFile(defaultClientImagePath);
                }
           }
        }
        
        
        public ICommand CreateOrderCommand { get;private set; }
        public ICommand PageCommand { get;private set; }
        public ICommand DataClicked { get;private set; }
        public ClientViewModel(MapView mapview, ClientModel client)
        {
            MapController = new(mapview);
            User = client;
            PageCommand = new Command(obj =>
            {
                Shell.Current.GoToAsync("//ClientAccount");
            });
        }
        public ClientViewModel()
        {
        }
    }
}
