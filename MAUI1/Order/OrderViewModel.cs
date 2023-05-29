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
        private DriverViewModel _driverViewModel;
        public DriverViewModel DriverVM 
        {
            get => _driverViewModel; 
            set 
            {
                if(value != null)
                {
                    _driverViewModel = value;
                    OnPropertyChanged(nameof(DriverFullName));
                    OnPropertyChanged(nameof(DriverPhoneNumber));
                    OnPropertyChanged(nameof(DriverEmail));
                    if (Order != null)
                    {
                        Order.Status = User.Order.OrderStatusType.InProgress;
                        OnPropertyChanged(nameof(OrderStatus));
                    }
                }
            }
        }
        public TaxiDispatcherModel Dispatcher { get; set; }
        public OrderModel Order { get; set; }
        public ICommand AcceptOrderCommand { get; set; }
        public ICommand DeclineOrderCommand { get; set; }
        public ICommand CompleteWithQuestionMarkCommand { get;set; }
        public ICommand CompleteOrderCommand { get; set; }
        public ICommand UndoCompletionCommand { get; set; }
        public string ClientName => ClientVM.UserFullName;
        public string ClientEmail => ClientVM.UserEmail;
        public string ClientPhone => ClientVM.UserPhoneNumber;
        public string StartingPoint
        {
            get => Order.StartingPoint;
            set 
            {
                if (value != null)
                {
                    Order.StartingPoint = value;
                    OnPropertyChanged("StartingPoint");
                }
            }
        }
        public string EndingPoint
        {
            get => Order.EndingPoint;
            private set
            {
                if (value != null)
                {
                    Order.StartingPoint = value;
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
                if (Order.Status == MAUI1.User.Order.OrderStatusType.Waiting)
                {
                    return "Ожидает";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatusType.InProgress)
                {
                    return "Выполняется";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatusType.CompletedWithQuestionMark)
                {
                    return "Завершён?";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatusType.Completed)
                {
                    return "Завершён";
                }
                else if(Order.Status == MAUI1.User.Order.OrderStatusType.Cancelled)
                {
                    return "Отменён";
                }
                else
                {
                    return "Неизвестен";
                }
            }
        }
        public string DriverFullName
        {
            get => DriverVM.UserFullName;
        }
        public string DriverPhoneNumber
        {
            get => DriverVM.UserPhoneNumber;
        }
        public string DriverEmail
        {
            get => DriverVM.UserEmail;
        }
        public OrderViewModel(OrderModel order, ClientViewModel clientVM, DriverViewModel driverVM)
        {
            Order = order;
            ClientVM = clientVM;
            DriverVM = driverVM;
            ClientVM.Order = this;
            DriverVM.Order = this;
            OrderViewModelCommandsInit();
        }
        public OrderViewModel(OrderModel order, ClientViewModel cvm)
        {
            Order = order;
            ClientVM = cvm;
            ClientVM.Order = this;
            OrderViewModelCommandsInit();
        }
        public void OrderViewModelCommandsInit()
        {
            AcceptOrderCommand = new Command(() => 
            { 
                Order.Status = MAUI1.User.Order.OrderStatusType.InProgress; OnPropertyChanged(nameof(OrderStatus));
            });
            DeclineOrderCommand = new Command(() => 
            { 
                Order.Status = MAUI1.User.Order.OrderStatusType.Cancelled; OnPropertyChanged(nameof(OrderStatus)); 
            });
            CompleteOrderCommand = new Command(() => 
            { 
                Order.Status = MAUI1.User.Order.OrderStatusType.Completed; OnPropertyChanged(nameof(OrderStatus)); 
            });
        }
    }
}



