using Mapsui.UI.Maui;
using MAUI1.User;
using MAUI1.User.Admin;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher;
using MAUI1.User.Dispatcher.Orders;
using MAUI1.User.Driver;
using MAUI1.User.Order;
using MAUI1.User.Registration;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MAUI1;

public partial class App : Application
{
    public static string projectPersonalFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MAUI1\\Images\\User1");
    public  App()
	{
	    InitializeComponent();
        //var dvm = new DriverViewModel();
        //Application.Current.UserAppTheme = AppTheme.Dark;
        //ClientViewModel client1 = new() { User = new("client", "client", "89130581262", "client@mail.ru", "client", UserType.Client) };
        //dvm.User = new("driver", "driver", "89130581263", "driver@mail.ru", "driver", UserType.Driver);
        //var order = new OrderModel("Улица Киренского 26, Красноярск", "Проспект Свободный 76Н", client1.User);
        //dvm.Order = new OrderViewModel(order, client1);
        ////MainPage = new DriverPage(dvm);
        //MainPage = new AppShell();
        //string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1");
        //Directory.CreateDirectory(path);
        //TaxiDispatcherModel tdispatcher = new TaxiDispatcherModel("dispatcher", "dispatcher", "dispatcher", "dispatcher", "dispatcher");
        //TaxiDispatcherViewModel taxiDispatcherViewModel = new TaxiDispatcherViewModel(tdispatcher);
        //MainPage = new TaxiDispatcherPage(taxiDispatcherViewModel);
        
        MainPage = new LoadingPage();
        //MainThread.BeginInvokeOnMainThread(async () =>
        //{
        //    var loc = await TCPCLient.GetUserLocation("driver");
        //    //System.Timers.Timer timer = new System.Timers.Timer();
        //    //timer.Interval = 1000;
        //    //Random rnd = new Random();
        //    //timer.Elapsed += async (o, e) =>
        //    //{

        //    //};
        //    //timer.Start();
        //});
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                if(true)
                {
                    var userVM = await TCPCLient.GetAutorizationDataAsync();
                    if (userVM != null)
                    {
                        if (userVM.User.UserType == UserType.Client)
                        {
                            MainPage = new ClientPage(userVM as ClientViewModel);
                        }
                        else if (userVM.User.UserType == UserType.Driver)
                        {
                            MainPage = new DriverPage(userVM as DriverViewModel);
                        }
                        else if (userVM.User.UserType == UserType.Dispatcher)
                        {
                            MainPage = new TaxiDispatcherPage(userVM as TaxiDispatcherViewModel);
                        }
                        else if (userVM.User.UserType == UserType.Administrator)
                        {
                            MainPage = new AdminPage(userVM as AdminVM);
                        }
                    }
                    else
                    {
                        MainPage = new AppShell();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        });
        Directory.CreateDirectory(projectPersonalFolderPath);
    }
}
