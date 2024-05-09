using NiisMenu.Builder;
using NiisMenu.Database;
using Plugin.CloudFirestore;

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
            CrossCloudFirestore.Current
                .Instance
                .Collection("Menu")
                .AddSnapshotListener((snapshot, error) =>
                {
                    if (snapshot != null && !snapshot.IsEmpty)
                    {
                        // Reload pending orders when there's a change in the database
                        LoadMenuItems();
                    }
                    else if (error != null)
                    {
                        Console.WriteLine($"Error listening for updates: {error}");
                    }
                });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadMenuItems();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            TableNumberPicker.SelectedItem = null;
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

        
        private bool ButtonEnabled = true;
        private async void OrderButtonClicked(object sender, EventArgs e)
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
                if (TableNumberPicker.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Please select a table number", "OK");
                    return;
                }
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
                    TableNumber = TableNumberPicker.SelectedItem.ToString(),
                    MenuName = menuItem.Name,
                    Quantity = tempQuantity,
                    Price = totalPrice,
                    TimeStamp = DateTime.UtcNow.ToString()
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

        private void Image_DescendantAdded(object sender, ElementEventArgs e)
        {

        }
    }
}
