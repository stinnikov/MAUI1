using MAUI1.User.Client;
using MAUI1.User.Dispatcher;
using MAUI1.User.Driver;
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
                //int d = 1;
                //if(d == 1)
                //Application.Current.MainPage = new NavigationPage(new Dispatcher.TaxiDispatcherPage());
                //if(d == 0)
                //{
                //    Application.Current.MainPage = new NavigationPage(new User.Driver.DriverPage());
                //}
                object[] loginStrings = (object[])obj;
                string login = loginStrings[0].ToString();
                string password = loginStrings[1].ToString();
                string response = null;
                if (login != null && password != null)
                {
                   response = await TCPCLient.SendLoginQueryToServerAsync(login, password);
                }
                if (response != null)
                {
                    if (response != null)
                    {
                        if (response == "Client")
                        {
                            Application.Current.MainPage = new ClientPage();
                        }
                        else if(response == "Driver")
                        {
                            Application.Current.MainPage = new DriverPage(new DriverViewModel());
                        }
                        else if(response == "Dispatcher")
                        {
                            Application.Current.MainPage = new TaxiDispatcherPage();
                        }
                        else if(response == "Administrator")
                        {
                            Application.Current.MainPage = new Admin.AdminPage();
                        }
                        else
                        {
                            
                        }
                    }
                    else
                    {
                        //(loginStrings[0] as Entry).Text = "";
                        //(loginStrings[1] as Entry).Text = "";
                    }
                }
            });
        }
    }
}
