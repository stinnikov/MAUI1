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

namespace MAUI1;

public partial class App : Application
{
    public static string projectPersonalFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MAUI1\\Images\\User1");
    public  App()
	{
	    InitializeComponent();
        var dvm = new DriverViewModel();
        Application.Current.UserAppTheme = AppTheme.Dark;
        ClientViewModel client1 = new() { User = new("Karlo", "Client", "89130581262", "client@mail.ru", "dadada", User.UserType.Client) };
        dvm.User = new("Karlo", "Driver", "89130481262", "driver1@mail.ru", "dadada", User.UserType.Driver);
        dvm.Order = new OrderViewModel(client1, dvm, "Красноярск, Институт космических и информационных технологий", "Красноярск, проспект Свободный 76");
        MainPage = new DriverPage(dvm);
        //MainPage = new OrderHandlerPage();
        //string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1");
        //Directory.CreateDirectory(path);
        Directory.CreateDirectory(projectPersonalFolderPath);
    }
    //protected override void OnStart()
    //{
    //    var task = TCPCLient.GetAutorizationDataAsync();

    //    task.ContinueWith((task) =>
    //    {
    //        MainThread.BeginInvokeOnMainThread(() =>
    //        {
    //            try
    //            {
    //                var userVM = task.Result;
    //                if (userVM != null)
    //                {
    //                    if (userVM.User.UserType == UserType.Client)
    //                    {
    //                        MainPage = new ClientPersonalAccountPage();
    //                    }
    //                    else if(userVM.User.UserType == UserType.Driver)
    //                    {
    //                        MainPage = new DriverPage();
    //                    }
    //                    else if(userVM.User.UserType == UserType.Dispatcher)
    //                    {
    //                        MainPage = new TaxiDispatcherPage();
    //                    }
    //                    else if (userVM.User.UserType == UserType.Administrator)
    //                    {
    //                        MainPage = new AdminPage();
    //                    }
    //                }
    //                else
    //                {
    //                    MainPage = new AppShell();
    //                }
    //            }
    //            catch(Exception ex)
    //            {
    //                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
    //            }
    //        });
    //    });

    //    base.OnStart();
    //}
}
