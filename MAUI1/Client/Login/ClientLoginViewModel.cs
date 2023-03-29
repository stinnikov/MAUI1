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
            LoginCommand = new Command(data =>
            { 
                //TODO:замутить логин
            });
        }
    }
}
