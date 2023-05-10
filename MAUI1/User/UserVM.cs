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
    class UserVM : INotifyPropertyChanged
    {
        private ImageSource _selectedImage;
        //private ImageSource avatarImage;
        public ICommand SelectImageCommand { get; private set; }
        public ICommand ReceiveImageCommand { get; private set; }

        public ImageSource SelectedImage
        {
            get { return _selectedImage; }
            set { _selectedImage = value; OnPropertyChanged("SelectedImage"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
