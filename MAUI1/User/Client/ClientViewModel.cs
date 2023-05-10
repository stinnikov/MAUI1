using Mapsui.UI.Maui;
using MAUI1.User.Dispatcher.Orders;
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
    internal class ClientViewModel : UserVM
    {
        private MapController MapController;
        public ClientModel Client { get; set; }
        public OrderViewModel ClientOrder { get; set; }
        public string ClientFirstName
        {
            get => Client.FirstName ?? "Имя";
            private set
            {
                if (IsNameValid(value))
                {
                    Client.FirstName = value;
                    OnPropertyChanged("ClientFirstName");
                }
            }
        }
        public string ClientLastName
        {
            get => Client.LastName ?? "Фамилия";
            private set
            {
                if (IsNameValid(value))
                {
                    Client.LastName = value;
                    OnPropertyChanged("ClientLastName");
                }
            }
        }
        public string ClientPatronymic
        {
            get => Client.Patronymic ?? "Отчество";
            private set
            {
                if (IsNameValid(value))
                {
                    Client.Patronymic = value;
                    OnPropertyChanged("ClientPatronymic");
                }
            }
        }
        public string ClientFullName
        {
            get
            {
                return $"{this.ClientFirstName} {this.ClientLastName} {this.ClientPatronymic}";
            }
        }
        public string ClientPhoneNumber
        {
            get => Client.PhoneNumber;
            private set
            {
                //TODO:проверка телефона клиента
                Client.PhoneNumber = value;
                OnPropertyChanged("ClientPhoneNumber");
            }
        }
        public string ClientEmail
        {
            get => Client.Email ?? "Email";
            private set
            {
                //TODO:проверка емейла
                Client.Email = value;
                OnPropertyChanged("ClientEmail");
            }
        }
        
        public ImageSource ClientAvatarSource
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
            
        

        public ICommand PageCommand { get; set; }
        public ICommand DataClicked { get; set; } 
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
