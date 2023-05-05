using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User
{
    public class UserModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _name;
        private string _email;
        private string _number;
        private string _password;

        private DateTime _created;
        public string Name { get { return _name; } set { if (value != null) { _name = value; OnPropertyChanged("Name"); } } }
        public string Number { get { return _number; } set { if (value != null) { _number = value; OnPropertyChanged("Number"); } } }
        public string Email { get { return _email; } set { if (value != null) { _email = value; OnPropertyChanged("Email"); } } }
        public string Password { get { return _password; } set { if (value != null) { _password = value; OnPropertyChanged("Password"); } } }
        public DateTime Created { get { return _created; } set { _created = value; OnPropertyChanged("Created"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
