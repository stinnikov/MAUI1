using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.Client.Login
{
    internal class ClientLoginViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ClientLoginViewModel() 
        {
            LoginCommand = new Command( async data =>
            {
                object[] entries = (data as object[]);
                string number = entries[0].ToString();
                string password = entries[1].ToString();
                string response = await TCPCLient.SendQueryToServer(new string[] { "LOGIN", number, password, "END" });
                if(response == "1")
                {
                    await Shell.Current.GoToAsync("//ClientAccount");
                }
                //TODO:замутить логин
            });
        }
    }
}
