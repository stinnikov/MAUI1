using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Driver
{
    internal class DriverModel : UserModel, INotifyPropertyChanged
    {
        private Order _order;
        public Order Order { get { return _order; } set { if (value != null) { _order = value; OnPropertyChanged("Order"); } } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public DriverModel()
        {

        }
        public DriverModel(Order order)
        {
            Order = order;
        }
    }
}
