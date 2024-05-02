using Microsoft.Maui.Controls;
using NiisMenu.AdminPages;
using Plugin.CloudFirestore;
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
                // Query Firestore to check if the provided credentials exist in the database
                var querySnapshot = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("Account")
                    .WhereEqualsTo("Username", username)
                    .WhereEqualsTo("Password", password)
                    .GetAsync();

                // Check if any document matched the query
                if (querySnapshot.Documents.Count() > 0)
                {
                    // Credentials are valid, navigate to the admin shell or perform other actions
                    App.Current.MainPage = new AdminShell();
                }
                else
                {
                    // Invalid credentials, display an alert
                    await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Login Failed", "Please enter username and password.", "OK");
            }
        }

    }
}
