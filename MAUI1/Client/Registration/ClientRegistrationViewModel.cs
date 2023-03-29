using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.Client.Registration
{
    internal class ClientRegistrationViewModel
    {
        public TCPCLient TCPCLient { get; set; } = new TCPCLient();
        public ObservableCollection<ClientUser> Clients { get; set; } = new();
        public RegistrationContext registrationContext = new();
        public ICommand RegistrationCommand { get; set; }
        public ClientRegistrationViewModel()
        {
            Linker.ViewModels.Add(this);
            
            Clients = registrationContext.Clients.Local.ToObservableCollection();
            RegistrationCommand = new Command(obj =>
            {
                object[] objects = (obj as object[]);
                string name = objects[0].ToString();
                string number = objects[1].ToString();
                string email = objects[2].ToString();
                string password = objects[3].ToString();
                string passwordConf = objects[4].ToString();
                bool isEmailValid = false;
                bool isPasswordValid = false;
                bool isNumberValid = false;
                string pattern = @"\D";
                string target = "";
                Regex regex = new Regex(pattern); //TODO:валидация телефона
                number = regex.Replace(number, target);
                if (Regex.IsMatch(email, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", RegexOptions.IgnoreCase))
                {
                    //TODO:проверка на суещствование зареганного емейла
                    //TODO:проверка существования почты
                    if (!Clients.Any(item => item.Email.Equals(email)))
                    {
                        isEmailValid = true;
                    }
                }
                if (password.Equals(passwordConf))
                {
                    isPasswordValid = true;
                }
                if(Regex.IsMatch(number, "[7-8]{1}[0-9]{10}", RegexOptions.IgnoreCase))
                {
                    if (!Clients.Any(item => item.Number.Equals(number)))
                    {
                        isNumberValid = true;
                    }
                }
                if(isEmailValid && isPasswordValid && isNumberValid)
                {
                    TCPCLient.SendQueryToServer(new string[] { name, number, email, password, "END"});
                }
            });
        }
    }
}
