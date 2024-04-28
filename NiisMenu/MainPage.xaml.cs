using Microsoft.Maui.Controls;

namespace NiisMenu
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the login page
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
