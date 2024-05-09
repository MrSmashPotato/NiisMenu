namespace NiisMenu
{
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private bool ButtonEnabled = true;
        private async void OnAddMenuItemClicked(object sender, EventArgs e)
        {
            if (!ButtonEnabled)
            {
                // Button is disabled, return
                return;
            }
            ButtonEnabled = false;
            try
            {

                await Navigation.PushAsync(new AddMenu());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                ButtonEnabled = true;
            }
            // Navigate to the AddMenuItemPage
        }

        private async void ViewMenu(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListMenu());
        }
    }
}
