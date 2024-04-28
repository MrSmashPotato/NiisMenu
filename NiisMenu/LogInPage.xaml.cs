using Microsoft.Maui.Controls;
using System;

namespace NiisMenu
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Here you can add your login logic.
            // For demonstration purposes, let's just check if username and password are not empty.
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                // If login successful, navigate to the main page
                await Navigation.PushAsync(new AdminPage());
            }
            else
            {
                await DisplayAlert("Login Failed", "Please enter username and password.", "OK");
            }
        }
    }
}
