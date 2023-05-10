using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher.Orders
{
    internal class OrderViewModel
    {
        public ObservableCollection<OrderModel> Orders { get; set; }
    }
}
