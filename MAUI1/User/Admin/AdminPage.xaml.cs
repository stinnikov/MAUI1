using MAUI1.User.Login;

namespace MAUI1.User.Admin;

public partial class AdminPage : ContentPage
{
	public AdminPage(AdminVM adminVM)
	{
		InitializeComponent();
		this.BindingContext = adminVM;
		adminVM.Navigation = this.Navigation;
		adminVM.Poll();
	}

    private async void LogoutClicked(object sender, EventArgs e)
    {
        var accepted = await App.Current.MainPage.DisplayAlert("Выход из аккаунта", "Вы уверены?", "Да", "Нет");
        if (accepted)
        {
            if (File.Exists(TCPCLient.accessTokenPath))
            {
                File.Delete(TCPCLient.accessTokenPath);
                var currentPage = this;
                var loginPage = new LoginPage();
                this.Navigation.InsertPageBefore(loginPage, this);
                await this.Navigation.PopAsync();
                this.Navigation.RemovePage(this);
            }
        }
    }
}