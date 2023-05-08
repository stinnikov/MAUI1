﻿using MAUI1.User;
using MAUI1.User.Registration;

namespace MAUI1;

public partial class App : Application
{
    public static string projectPersonalFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MAUI1\\Images\\User1");
    public App()
	{
	    InitializeComponent();
		MainPage = new UserAccountPage();
        //string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1");
        //Directory.CreateDirectory(path);
        Directory.CreateDirectory(projectPersonalFolderPath);
    }
}
