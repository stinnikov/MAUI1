using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User.Registration
{
    internal class UserRegistrationViewModel
    {
        //public RegistrationContext registrationContext = new();
        public ICommand RegistrationCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand TestCommand1 { get; set; }
        public UserRegistrationViewModel()
        {
            Linker.ViewModels.Add(this);
            RegistrationCommand = new Command(async obj =>
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
                    isEmailValid = true;
                }
                if (password.Equals(passwordConf))
                {
                    //TODO: проверка на парольчик
                    isPasswordValid = true;
                }
                if(Regex.IsMatch(number, "[7-8]{1}[0-9]{10}", RegexOptions.IgnoreCase))
                {
                    isNumberValid = true;
                }
                if(isEmailValid && isPasswordValid && isNumberValid)
                {
                   var response = await MAUI1.TCPCLient.SendQueryToServer(new string[] {"REGADD", name, number, email, password, "END"}, 8888, "192.168.0.36");
                    //TODO: написать что регистрация прошла успешна и попросить залогиниться если 1, если 0 то хз пока
                }
            });
          

        }
    }
}
