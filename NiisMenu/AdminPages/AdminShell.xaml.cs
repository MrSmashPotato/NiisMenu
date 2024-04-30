namespace NiisMenu.AdminPages
{
    public partial class AdminShell : Shell
    {
        public AdminShell()
        {
            InitializeComponent();
        }
        private async void OnLogOutClicked(object sender, EventArgs e)
        {
            bool logoutConfirmed = await DisplayAlert("Log Out", "Are you sure you want to log out?", "Yes", "No");

            if (logoutConfirmed)
            {
                // Perform log out actions here
                // For example, navigate to the login page and clear user data
                App.Current.MainPage = new AppShell();
            }
        }
    }
}
