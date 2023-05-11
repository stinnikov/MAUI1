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

namespace MAUI1.User.Driver
{
    internal class DriverViewModel : UserVM
    {
        public MapController MapController { get; set; }
        private DriverModel _driver = new DriverModel();
        public DriverModel Driver
        {
            get => _driver;
            set
            {
                if(value != null)
                {
                    _driver = value;
                    OnPropertyChanged("Driver");
                }
            }
        }
        public OrderViewModel DriverOrder { get; set; }
        public string DriverFirstName
        {
            get => Driver.FirstName ?? "Имя";
            private set
            {
                if (IsNameValid(value))
                {
                    Driver.FirstName = value;
                    OnPropertyChanged("DriverFirstName");
                }
            }

        }
        public string DriverLastName
        {
            get => Driver.LastName ?? "Фамилия";
            private set
            {
                if (IsNameValid(value))
                {
                    Driver.LastName = value;
                    OnPropertyChanged("DriverLastName");
                }
            }
        }
        public string DriverPatronymic
        {
            get => Driver.Patronymic ?? "Отчество";
            private set
            {
                if (IsNameValid(value))
                {
                    Driver.Patronymic = value;
                    OnPropertyChanged("DriverPatronymic");
                }
            }
        }
        public string DriverFullName
        {
            get
            {
                if (Driver.Patronymic != "")
                {
                    return $"{this.DriverFirstName} {this.DriverLastName} {this.DriverPatronymic}";
                }
                return $"{this.DriverFirstName} {this.DriverLastName}";
            }
        }
        public string DriverPhoneNumber
        {
            get => Driver.PhoneNumber;
            private set
            {
                //TODO:проверка телефона клиента
                Driver.PhoneNumber = value;
                OnPropertyChanged("DriverPhoneNumber");
            }
        }
        public string DriverEmail
        {
            get => Driver.Email ?? "Email";
            private set
            {
                //TODO:проверка емейла
                Driver.Email = value;
                OnPropertyChanged("DriverEmail");
            }
        }

        public ImageSource DriverAvatarSource
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
        public int DriverAge => Driver.Age;
        public ICommand camanda { get; set; }

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
                var d = this;
            });
        }
    }
}

