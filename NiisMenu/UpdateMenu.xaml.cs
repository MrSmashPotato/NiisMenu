using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using NiisMenu.Builder;
using Plugin.CloudFirestore;

namespace NiisMenu;

public partial class UpdateMenu : ContentPage
{
	public UpdateMenu(Menu selectedMenu)
	{
		InitializeComponent();
        NameEntry.Text = selectedMenu.Name;
        PriceEntry.Text = selectedMenu.Price.ToString();
        CategoryPicker.SelectedItem = selectedMenu.Category;
        AvailableSwitch.IsToggled = selectedMenu.IsAvailable;
        
	}
   
    private async void OnUpdateItemClicked(object sender, EventArgs e)
    {
        try
        {
            Menu Newmenu = new Menu
            {
                Name = NameEntry.Text,
                Price = Convert.ToInt32(PriceEntry.Text),
                Category = CategoryPicker.SelectedItem.ToString(),
                IsAvailable = AvailableSwitch.IsToggled,
                Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
                Time = TimeOnly.FromDateTime(DateTime.Now).ToString()
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

    }
    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
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
    }



}