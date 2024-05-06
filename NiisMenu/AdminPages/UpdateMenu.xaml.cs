using NiisMenu.Builder;
using Plugin.CloudFirestore;
using Plugin.FirebaseStorage;

namespace NiisMenu
{
    public partial class UpdateMenu : ContentPage
    {
        private string existingImageUrl;

        public UpdateMenu(Menu selectedMenu)
        {
            InitializeComponent();
            NameEntry.Text = selectedMenu.Name;
            PriceEntry.Text = selectedMenu.Price.ToString();
            CategoryPicker.SelectedItem = selectedMenu.Category;
            AvailableSwitch.IsToggled = selectedMenu.IsAvailable;

            existingImageUrl = selectedMenu.ImageUrl; // Assign selectedMenu's ImageUrl to existingImageUrl
        }
        private bool isButtonEnabled = true;
        private async void OnUpdateItemClicked(object sender, EventArgs e)
        {
            if (!isButtonEnabled)
            {
                // Button is disabled, return
                return;
            }
            isButtonEnabled = false;
            try
            {
                Menu Newmenu = new Menu
                {
                    Name = NameEntry.Text,
                    Price = Convert.ToInt32(PriceEntry.Text),
                    Category = CategoryPicker.SelectedItem.ToString(),
                    IsAvailable = AvailableSwitch.IsToggled,
                    Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    Time = TimeOnly.FromDateTime(DateTime.Now).ToString(),
                    ImageUrl = existingImageUrl // Use existingImageUrl here
                };

                var querySnapshot = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("Menu")
                    .WhereEqualsTo("Name", NameEntry.Text)
                    .GetAsync();

                foreach (var documentSnapshot in querySnapshot.Documents)
                {
                    var documentId = documentSnapshot.Id;

                    await CrossCloudFirestore.Current
                        .Instance
                        .Collection("Menu")
                        .Document(documentId)
                        .UpdateAsync(Newmenu);
                }
                MessagingCenter.Send(this, "DataUpdated");
                await DisplayAlert("Success", "Menu Updated", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                isButtonEnabled = true;
            }
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            if (!isButtonEnabled)
            {
                // Button is disabled, return
                return;
            }
            isButtonEnabled = false;
            try
            {
                var confirmDelete = await DisplayAlert("Confirm", "Are you sure you want to delete this menu item?", "Yes", "No");
                if (confirmDelete)
                {
                    var querySnapshot = await CrossCloudFirestore.Current
                        .Instance
                        .Collection("Menu")
                        .WhereEqualsTo("Name", NameEntry.Text)
                        .GetAsync();

                    foreach (var documentSnapshot in querySnapshot.Documents)
                    {
                        var documentId = documentSnapshot.Id;
                        await CrossCloudFirestore.Current
                            .Instance
                            .Collection("Menu")
                            .Document(documentId)
                            .DeleteAsync();
                    }

                    MessagingCenter.Send(this, "DataUpdated");
                    await DisplayAlert("Success", "Menu deleted successfully", "OK");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                isButtonEnabled = true;
            }
        }
        

    }
}
