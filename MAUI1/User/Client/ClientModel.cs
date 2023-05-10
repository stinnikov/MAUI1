using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Client
{
    internal class ClientModel : UserModel
    {
        public ClientModel() { }
        public ClientModel(string firstName, string lastName, string patronymic, string phoneNumber, string email, string password ) : base(firstName, lastName, patronymic, phoneNumber)
        {
        }
    }
}


