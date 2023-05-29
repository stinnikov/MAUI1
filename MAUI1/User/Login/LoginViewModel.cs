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
    internal class LoginViewModel
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
                            Application.Current.MainPage = new ClientPage(userVM as ClientViewModel);
                        }
                        else if (userVM.GetType() == typeof(DriverViewModel))
                        {
                            Application.Current.MainPage = new DriverPage(userVM as DriverViewModel);
                        }
                        else if (userVM.GetType() == typeof(TaxiDispatcherViewModel))
                        {
                            Application.Current.MainPage = new TaxiDispatcherPage(userVM as TaxiDispatcherViewModel);
                        }
                        else if (userVM.GetType() == typeof(AdminVM))
                        {
                            Application.Current.MainPage = new Admin.AdminPage(userVM as AdminVM);
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
