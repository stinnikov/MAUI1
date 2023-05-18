using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User
{
    public class UserVM : ViewModel, INotifyPropertyChanged
    {
        private ImageSource _selectedImage;
        public UserModel User { get; set; }
        //private ImageSource avatarImage;
        public string UserFirstName
        {
            get => User?.FirstName ?? "Имя";
            private set
            {
                if (IsNameValid(value))
                {
                    User.FirstName = value;
                    OnPropertyChanged("UserFirstName");
                }
            }

        }
        public string UserLastName
        {
            get => User?.LastName ?? "Фамилия";
            private set
            {
                if (IsNameValid(value))
                {
                    User.LastName = value;
                    OnPropertyChanged("UserLastName");
                }
            }
        }
        public string UserPatronymic
        {
            get => User?.Patronymic ?? "Отчество";
            private set
            {
                if (IsNameValid(value))
                {
                    User.Patronymic = value;
                    OnPropertyChanged("UserPatronymic");
                }
            }
        }
        public string UserFullName
        {
            get
            {
                if (User?.Patronymic != null)
                {
                    return $"{this.UserFirstName} {this.UserLastName} {this.UserPatronymic}";
                }
                return $"{this.UserFirstName} {this.UserLastName}";
            }
        }
        public string UserPhoneNumber
        {
            get => User?.PhoneNumber;
            private set
            {
                //TODO:проверка телефона клиента
                User.PhoneNumber = value;
                OnPropertyChanged("UserPhoneNumber");
            }
        }
        public string UserEmail
        {
            get => User?.Email ?? "Email";
            private set
            {
                //TODO:проверка емейла
                User.Email = value;
                OnPropertyChanged("UserEmail");
            }
        }
        public UserType UserAccountType
        {
            get => User.UserType;
            private set
            {
                //TODO:проверка емейла
                User.UserType = value;
                OnPropertyChanged("UserEmail");
            }
        }
        public ICommand SelectImageCommand { get; private set; }
        public ICommand AvatarClickedCommand { get; set; } = new Command(() => 
        { 

        });
        public ICommand ReceiveImageCommand { get; private set; }

        public ImageSource SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; OnPropertyChanged("SelectedImage"); }
        }
        public UserVM()
        {
            var personalFolderPath = $"{App.projectPersonalFolderPath}";
            SetAvatar(personalFolderPath);
            SelectImageCommand = new Command(() =>
            {
                ChooseImage();
            });
            ReceiveImageCommand = new Command(async () => { 
                await TCPCLient.ReceiveAvatar();
                SetAvatar(personalFolderPath);
            });
        }
        public UserVM(UserModel user)
        {
            User = user;
        }
        public bool IsNameValid(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
        public void GetAvatar()
        {
            //TODO:получение аватара юзера
        }
        private void SetAvatar(string personalFolderPath)
        {
            var avatarPath = Path.Combine(personalFolderPath, "avatar.png");
            if (File.Exists(avatarPath))
            {
                SelectedImage = ImageSource.FromFile(avatarPath);
            }
        }
        private async void ChooseImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.image" } },
                    { DevicePlatform.Android, new[] { "image/*" } },
                    { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".gif" } },
                })
                });

                if (result != null)
                {
                    bool accepted = await Application.Current.MainPage.DisplayAlert("Confirmation", "Do you want to save this image as your avatar?", "Yes", "No", FlowDirection.MatchParent);
                    if (accepted)
                    {
                        SelectedImage = ImageSource.FromStream(() => result.OpenReadAsync().Result);
                        try
                        {
                            var fileStream = new FileStream(result.FullPath, FileMode.Open, FileAccess.Read);
                            byte[] buffer = new byte[fileStream.Length];
                            fileStream.Read(buffer, 0, buffer.Length);
                            await MAUI1.TCPCLient.SendImage(buffer, fileStream.Length);
                        }
                        catch (Exception ex)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save the image: {ex.Message}", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to pick the image: {ex.Message}", "OK");
            }
        }
    }
}
