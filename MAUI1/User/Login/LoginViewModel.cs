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
        public LoginModel LoginInputs { get; set; }
        public ICommand LoginCommand { get; set; }
        public LoginViewModel() 
        {
            LoginInputs = new LoginModel();
            LoginCommand = new Command(async () =>
            {
                int d = 1;
                if(d == 1)
                Application.Current.MainPage = new NavigationPage(new Dispatcher.TaxiDispatcherPage());
                if(d == 0)
                {
                    Application.Current.MainPage = new NavigationPage(new User.Driver.DriverPage());
                }
                //List<string> response = await TCPCLient.SendQueryToServer(new string[] { "LOGIN", LoginInputs.Number, LoginInputs.Password, "END" });
                //if (response != null)
                //{
                //    if (response[0] == "1")
                //    {
                //        Application.Current.MainPage = new AppShell(response[1]);
                //    }
                //    else
                //    {
                //        LoginInputs.Number = "";
                //        LoginInputs.Password = "";
                //    }
                //}
            });
        }
    }
}
