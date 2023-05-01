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
            LoginCommand = new Command( async data =>
            {
                //object[] entries = (data as object[]);
                //string number = entries[0].ToString();
                //string password = entries[1].ToString();
                //string response = await TCPCLient.SendQueryToServer(new string[] { "LOGIN", number, password, "END" });
                //if(response == "1")
                //{
                //    await Shell.Current.GoToAsync("//ClientAccount");
                //}
                Application.Current.MainPage = new AppShell();
                //TODO:замутить логин
            });
        }
    }
}
