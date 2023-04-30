using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Driver
{
    internal class DriverAccountViewModel
    {
        public DriverModel Driver { get; set; } = new DriverModel();
        public DriverAccountViewModel()
        {
            Driver.Order = new Order(DateTime.Now, 5, "chel", "chel2", 1000, Driver);
        }
    }
}

