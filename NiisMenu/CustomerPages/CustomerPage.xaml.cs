using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Plugin.CloudFirestore;
using NiisMenu.Builder;
using NiisMenu.Database;

namespace NiisMenu
{
    public partial class CustomerPage : ContentPage
    {
        private readonly DatabaseHandler menuDatabase;

        public CustomerPage()
        {
            InitializeComponent();
            CategoryPicker.SelectedIndexChanged += async (sender, e) => await LoadMenuItems();
            menuDatabase = new DatabaseHandler();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadMenuItems();
        }

        private async Task LoadMenuItems()
        {
            string selectedCategory = CategoryPicker.SelectedItem as string;
            if (selectedCategory == null)
            {
                List<Menu> menuItems = await menuDatabase.GetMenuItems();
                MenuListView.ItemsSource = menuItems;
            }

            if (selectedCategory != null)
            {
                List<Menu> menuItems = await menuDatabase.GetMenuItems(selectedCategory);
                MenuListView.ItemsSource = menuItems;
            }
        }

        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            await LoadMenuItems();
        }
        private bool ButtonEnabled = true;
        private async void OrderButton_Clicked(object sender, EventArgs e)
        {
            if (!ButtonEnabled)
            {
                // Button is disabled, return
                return;
            }

            ButtonEnabled = false;
            try
            {
                var menuItem = (Menu)((Button)sender).CommandParameter;

                // Prompt the user for the quantity
                string quantityInput = await DisplayPromptAsync("Order Quantity", "Enter the quantity:", initialValue: "", maxLength: 3, keyboard: Keyboard.Numeric);

                // Check if the user canceled the prompt or entered an empty value
                if (string.IsNullOrWhiteSpace(quantityInput))
                    return;

                // Parse the quantity input to an integer
                if (!int.TryParse(quantityInput, out int tempQuantity))
                {
                    await DisplayAlert("Error", "Please enter a valid integer quantity.", "OK");
                    return;
                }

                // Calculate the price based on the quantity
                int totalPrice = menuItem.Price * tempQuantity;

                // Prompt the user with the total price before confirming the order
                bool confirmOrder = await DisplayAlert("Confirm Order", $"Total Price: {totalPrice}. Do you want to place the order?", "Yes", "No");
                if (!confirmOrder)
                    return;

                // Create the order
                Order newOrder = new Order
                {
                    TableNumber = 1,
                    MenuName = menuItem.Name,
                    Quantity = tempQuantity,
                    Price = totalPrice,
                    Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    Time = TimeOnly.FromDateTime(DateTime.Now).ToString()
                };

                // Add the order to the database
                await CrossCloudFirestore.Current
                    .Instance
                    .Collection("PendingOrder")
                    .AddAsync(newOrder);

                // Display success message
                await DisplayAlert("Success", "Order placed successfully", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                ButtonEnabled = true;
            }          
        }

    }
}
