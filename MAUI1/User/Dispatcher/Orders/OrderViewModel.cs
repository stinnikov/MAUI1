using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher.Orders
{
    internal class OrderViewModel : ViewModel
    {
        public ClientViewModel ClientVM { get; }
        public DriverViewModel DriverVM { get; set; }
        public TaxiDispatcherModel Dispatcher { get; set; }
        public OrderModel Order { get; set; }
        public string ClientName => ClientVM.ClientFullName;
        public string ClientEmail => ClientVM.ClientEmail;
        public string ClientPhone => ClientVM.ClientPhoneNumber;
        public string StartingPoint
        {
            get => Order.StartPoint;
            private set
            {
                if (value != null)
                {
                    Order.StartPoint = value;
                    OnPropertyChanged("StartingPoint");
                }
            }
        }
        public string EndingPoint
        {
            get => Order.EndPoint;
            private set
            {
                if (value != null)
                {
                    Order.StartPoint = value;
                    OnPropertyChanged("EndingPoint");
                }
            }
        }
        public float OrderPrice
        {
            get => Order.Price;
            private set
            {
                if(value >= 0)
                {
                    Order.Price = value;
                    OnPropertyChanged("OrderPrice");
                }
            }
        }
        public string OrderStatus => Order.Status.ToString(); 
        public OrderViewModel(ClientViewModel clientVM, DriverViewModel driverVM, string startPoint, string endPoint)
        {
            ClientVM = clientVM;
            DriverVM = driverVM;
            Order = new OrderModel()
            {
                Client = clientVM.Client,
                Driver = driverVM.Driver,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
        }
        public OrderViewModel(ClientViewModel clientVM, string startPoint, string endPoint)
        {
            ClientVM = clientVM;
            Order = new OrderModel()
            {
                Client = clientVM.Client,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
        }
        public OrderViewModel()
        {

        }
    }
}



