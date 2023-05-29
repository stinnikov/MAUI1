using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Admin
{
    public class AdminVM : UserVM
    {
        public ObservableCollection<UserVM> Users { get; set; } = new ObservableCollection<UserVM>();
        public ICommand Test { get; set; }
        public AdminVM(UserModel user)
        {
            User = user;
            Test = new Command(async () =>
            {
                var users = await TCPCLient.GetUsersFromServerAsync();
                foreach(var user in users)
                {
                    Users.Add(new UserVM(user));
                }
            });
        }

    }
}
