using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Client
{
    internal class ClientModel : UserModel, INotifyPropertyChanged
    {
        public ClientModel() { }
        public ClientModel(string name, string number, string email, string password ) 
        {
            Name = name;
            Number = number;
            Email = email;
            Password = password;
        }
    }
}


