using MAUI1.User.Admin;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher;
using MAUI1.User.Driver;
using OsmSharp.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Login
{
    internal class LoginViewModel : ViewModel
    {
        public ICommand LoginCommand { get; set; }
        public LoginViewModel() 
        {
            LoginCommand = new Command(async obj =>
            {
                object[] loginStrings = (object[])obj;
                string login = loginStrings[0].ToString();
                string password = loginStrings[1].ToString();
                if (login != null && password != null)
                {
                   var userVM = await TCPCLient.SendLoginQueryToServerAsync(login, password);
                    if (userVM != null)
                    {
                        if (userVM.GetType() == typeof(ClientViewModel))
                        {
                            var currentPage = this.Navigation.NavigationStack.Last();
                            this.Navigation.InsertPageBefore(new ClientPage(userVM as ClientViewModel), currentPage);
                            this.Navigation.PopAsync();
                            this.Navigation.RemovePage(currentPage);
                            //await this.Navigation.PushAsync(new ClientPage(userVM as ClientViewModel));
                        }
                        else if (userVM.GetType() == typeof(DriverViewModel))
                        {
                            Application.Current.MainPage = new NavigationPage(new DriverPage(userVM as DriverViewModel));
                        }
                        else if (userVM.GetType() == typeof(TaxiDispatcherViewModel))
                        {
                            Application.Current.MainPage = new NavigationPage(new TaxiDispatcherPage(userVM as TaxiDispatcherViewModel));
                        }
                        else if (userVM.GetType() == typeof(AdminVM))
                        {
                            Application.Current.MainPage = new NavigationPage(new Admin.AdminPage(userVM as AdminVM));
                        }
                        else
                        {
                        }

                    }
                }
                
            });
        }
    }
}
