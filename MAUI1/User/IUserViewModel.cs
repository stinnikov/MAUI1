using Mapsui.UI.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI1.User
{
    interface IUserViewModel
    {
        public ICommand AvatarClicked { get; set; }
    }
}
