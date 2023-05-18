using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Driver
{
    public class DriverModel : UserModel
    {
        public DriverModel(string firstName, string lastName, string phoneNumber, string email, string password, UserType userType) : base(firstName, lastName, phoneNumber, email, password, userType)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }
}
