using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.Client
{
    internal class ClientViewModel : INotifyPropertyChanged
    {
        public ICommand PageCommand { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ClientViewModel()
        {
            PageCommand = new Command(obj => 
            { 
                Shell.Current.GoToAsync("//ClientAccount");
            });
        }
    }
}
