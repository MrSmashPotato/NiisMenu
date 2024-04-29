using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using NiisMenu.Builder;
using Plugin.CloudFirestore;

namespace NiisMenu
{
    public partial class AddMenu : ContentPage
    {
        public AddMenu()
        {
            InitializeComponent();
        }

        private async void OnAddItemClicked(object sender, EventArgs e)
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

                await CrossCloudFirestore.Current
                    .Instance
                    .Collection("Menu")
                    .AddAsync(Newmenu);
                await DisplayAlert("Success", "Menu item added successfully", "OK");
                ClearFields();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            
        }

        private void ClearFields()
        {
            NameEntry.Text = "";
            PriceEntry.Text = "";
            CategoryPicker.SelectedIndex = 0;
            AvailableSwitch.IsToggled = true;
        }
    }
}
