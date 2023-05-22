using Mapsui.UI.Maui;
using MAUI1.User.Client;
using MAUI1.User.Driver;
using MAUI1.User.Dispatcher.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MAUI1.User.Dispatcher;

namespace MAUI1.User.Order
{
    public class OrderViewModel : ViewModel
    {
        public ClientViewModel ClientVM { get; }
        public DriverViewModel DriverVM { get; set; }
        public TaxiDispatcherModel Dispatcher { get; set; }
        public OrderModel Order { get; set; }
        public ICommand AcceptOrderCommand { get; set; }
        public ICommand DeclineOrderCommand { get; set; }
        public ICommand CompleteWithQuestionMarkCommand { get;set; }
        public ICommand CompleteOrderCommand { get; set; }
        public ICommand UndoCompletionCommand { get; set; }
        public string ClientName => ClientVM.UserFirstName;
        public string ClientEmail => ClientVM.UserEmail;
        public string ClientPhone => ClientVM.UserPhoneNumber;
        public string StartingPoint
        {
            get => Order.StartPoint;
            set 
            {
                if (value != null)
                {
                    Order.StartPoint = value;
                    OnPropertyChanged("StartingPoint");
                    SetLonLatProp(value);
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
        public string OrderStatus 
        {
            get
            {
                if (Order.Status == MAUI1.User.Order.OrderStatus.Waiting)
                {
                    return "Ожидает";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatus.InProgress)
                {
                    return "Выполняется";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatus.CompletedWithQuestionMark)
                {
                    return "Завершён?";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatus.Completed)
                {
                    return "Завершён";
                }
                else
                {
                    return "Отменён";
                }
            }
        }
        public (double, double) StartingPointLonLat { get; set; }
        public async void SetLonLatProp(string address)
        {
            StartingPointLonLat = await MapController.GetLonLatFromAddress(address);
        }
        public OrderViewModel(ClientViewModel clientVM, DriverViewModel driverVM, string startPoint, string endPoint)
        {
            ClientVM = clientVM;
            DriverVM = driverVM;
            Order = new OrderModel()
            {
                Client = clientVM.User,
                Driver = driverVM.User,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
            AcceptOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.InProgress; OnPropertyChanged("OrderStatus"); });
            DeclineOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.Cancelled; OnPropertyChanged("OrderStatus"); });
            CompleteWithQuestionMarkCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.CompletedWithQuestionMark; OnPropertyChanged("OrderStatus"); });
            CompleteOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.Completed; OnPropertyChanged("OrderStatus"); });
            UndoCompletionCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.InProgress; OnPropertyChanged("OrderStatus"); });
        }
        public OrderViewModel(ClientViewModel clientVM, string startPoint, string endPoint)
        {
            ClientVM = clientVM;
            Order = new OrderModel()
            {
                Client = clientVM.User,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
            AcceptOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.InProgress; OnPropertyChanged("OrderStatus"); });
            DeclineOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.Cancelled; OnPropertyChanged("OrderStatus"); });
            CompleteWithQuestionMarkCommand = new Command(() => 
            { Order.Status = MAUI1.User.Order.OrderStatus.CompletedWithQuestionMark; OnPropertyChanged("OrderStatus"); });
            CompleteOrderCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.Completed; OnPropertyChanged("OrderStatus"); });
            UndoCompletionCommand = new Command(() => { Order.Status = MAUI1.User.Order.OrderStatus.InProgress; OnPropertyChanged("OrderStatus"); });
        }
        public OrderViewModel()
        {

        }
    }
}



