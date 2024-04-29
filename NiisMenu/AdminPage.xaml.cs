using Microsoft.Maui.Controls;

namespace NiisMenu
{
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private async void OnAddMenuItemClicked(object sender, EventArgs e)
        {
            // Navigate to the AddMenuItemPage
            await Navigation.PushAsync(new AddMenu());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListMenu());
        }
    }
}
    