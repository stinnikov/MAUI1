using CommunityToolkit.Maui.Core.Extensions;
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
        public ObservableCollection<UserType> UserTypes { get; set; } = new ObservableCollection<UserType>() { UserType.Client, UserType.Driver, UserType.Dispatcher, UserType.Administrator };
        public ICommand Test { get; set; }
        public AdminVM(UserModel user)
        {
            User = user;
            Test = new Command(async () =>
            {
                string action = await Navigation.NavigationStack.LastOrDefault().DisplayActionSheet($"Выберите роль (Текущая роль:Driver)", "Назад", null, "Driver", "Client", "Dispatcher");
                if (action != "Назад")
                {
                }
            });
        }
        public new async void Poll()
        {
            var data = (await base.Poll() as UserVM[]);
            if (data != null)
            {
                this.Users = data.ToObservableCollection();
            }
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += (o, e) => { timer.Stop(); this.Poll(); };
            timer.Interval = 5 * 1000;
            timer.Start();
        }
    }
}
