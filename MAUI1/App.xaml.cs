namespace MAUI1;

public partial class App : Application
{
	public App()
	{
	    InitializeComponent();

		MainPage = new MAUI1.User.Login.LoginPage();
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1");
        Directory.CreateDirectory(path);
    }
}
