namespace MAUI1;

public partial class App : Application
{
	public App()
	{
	    InitializeComponent();

		MainPage = new AppShell("");
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"MAUI//MAUI1");
        Directory.CreateDirectory(path);
    }
}
