using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Client
{
    internal class ClientViewModel : UserVM,INotifyPropertyChanged
    {
        private MapController MapController;
        public ClientModel Client { get; set; }
        public ICommand PageCommand { get; set; }
        public ICommand AvatarClicked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICommand DataClicked { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ClientViewModel(MapView mapview)
        {
            MapController = new(mapview);
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
