using System;
using System.IO;
using Microsoft.Maui.Controls;
using NiisMenu.Builder;
using Plugin.CloudFirestore;
using Plugin.FirebaseStorage;

namespace NiisMenu
{
    public partial class AddMenu : ContentPage
    {
        private byte[] imageData;

        public AddMenu()
        {
            InitializeComponent();
        }
        private bool isBottonEnabled = true;
        private async void OnAddItemClicked(object sender, EventArgs e)
        {
            if (!isBottonEnabled)
            {
                return;
            }
            isBottonEnabled = false;
            try
            {
                var querySnapshot = await CrossCloudFirestore.Current
            .Instance
            .Collection("Menu")
            .WhereEqualsTo("Name", NameEntry.Text)
            .GetAsync();
                if (querySnapshot.Count > 0)
                {
                    await DisplayAlert("Error", "Menu item already exists", "OK");
                    return;
                }
                try
                {
                    // Upload image to Firebase Storage if imageData is not null
                    string imageUrl = null;
                    if (imageData != null)
                    {
                        // Generate a unique filename for the image
                        string imageName = Guid.NewGuid().ToString();
                        string imagePath = $"images/{imageName}.jpg";

                        // Upload image to Firebase Storage
                        imageUrl = await UploadImage(imageData, imagePath);
                    }

                    // Create a new Menu object with the provided details
                    Menu newMenu = new Menu
                    {
                        Name = NameEntry.Text,
                        Price = Convert.ToInt32(PriceEntry.Text),
                        Category = CategoryPicker.SelectedItem.ToString(),
                        IsAvailable = AvailableSwitch.IsToggled,
                        Date = DateTime.UtcNow.ToString(), // Use UTC time to ensure consistency across devices
                        ImageUrl = imageUrl // Save the image URL in the menu object
                    };

                    // Add the new menu item to Firestore
                    await CrossCloudFirestore.Current
                        .Instance
                        .Collection("Menu")
                        .AddAsync(newMenu);

                    await DisplayAlert("Success", "Menu item added successfully", "OK");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                isBottonEnabled = true;
            }
        }

        private async Task<string> UploadImage(byte[] imageData, string imagePath)
        {
            // Upload image to Firebase Storage
            await CrossFirebaseStorage.Current
                .Instance
                .RootReference
                .Child(imagePath)
                .PutBytesAsync(imageData);

            // Get download URL of the uploaded image
            var downloadUrl = await CrossFirebaseStorage.Current
                .Instance
                .RootReference
                .Child(imagePath)
                .GetDownloadUrlAsync();

            return downloadUrl.ToString();
        }

        private void ClearFields()
        {
            NameEntry.Text = "";
            PriceEntry.Text = "";
            CategoryPicker.SelectedIndex = 0;
            AvailableSwitch.IsToggled = true;
            ImagePreview.Source = null; // Clear the image preview
        }

        private async void OnSelectImageButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Open file picker to select image
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select Image"
                });

                if (result != null)
                {
                    // Read image data into byte array
                    using (var stream = await result.OpenReadAsync())
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }

                    // Display image preview
                    ImagePreview.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
