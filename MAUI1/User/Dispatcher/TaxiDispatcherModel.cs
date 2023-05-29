using MAUI1.User.Driver;
using MAUI1.User.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Dispatcher
{
    public class TaxiDispatcherModel : UserModel
    {
        public ObservableCollection<DriverViewModel> Drivers { get; set; } = new();
        public ObservableCollection<OrderViewModel> OrdersCollection { get; set; } = new ObservableCollection<OrderViewModel>();
        public static new UserType UserType { get; } = UserType.Dispatcher;
        public TaxiDispatcherModel(string firstName, string lastName, string phoneNumber, string email, string password) : base(firstName, lastName, phoneNumber, email, password, UserType)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
        }
    }
}
