using NiisMenu.AdminPages;
using NiisMenu.Builder;
using Plugin.CloudFirestore;
using Plugin.LocalNotification;

namespace NiisMenu
{
    public partial class PendingOrder : ContentPage
    {
        public PendingOrder()
        {
            InitializeComponent();
            Loaded += async (sender, e) => await LoadPendingOrders();

            // Listen for real-time updates
            CrossCloudFirestore.Current
                .Instance
                .Collection("PendingOrder")
                .AddSnapshotListener((snapshot, error) =>
                {
                    if (snapshot != null && !snapshot.IsEmpty)
                    {
                        // Reload pending orders when there's a change in the database
                        LoadPendingOrders();
                    }
                    else if (error != null)
                    {
                        Console.WriteLine($"Error listening for updates: {error}");
                    }
                });
        }
       
        public async Task LoadPendingOrders()
        {
            List<EditOrder> pendingOrders = await GetPendingOrders();
            ItemListView.ItemsSource = pendingOrders;
        }

        public async Task<List<EditOrder>> GetPendingOrders()
        {
            List<EditOrder> pendingOrders = new List<EditOrder>();
            var querySnapshot = await CrossCloudFirestore.Current
                .Instance
                .Collection("PendingOrder")
                .OrderBy("TimeStamp")
                .GetAsync();

            foreach (var document in querySnapshot.Documents)
            {
                EditOrder neworder = document.ToObject<EditOrder>();
                neworder.OrderID = document.Id;
                pendingOrders.Add(neworder);
            }


            return pendingOrders;
        }
        private bool isButtonEnabled = true;
        private async void DoneClicked(object sender, EventArgs e)
        {
            if (!isButtonEnabled)
            {
                // Button is disabled, return
                return;
            }
            isButtonEnabled = false;
            try
            {
                var orderhistory = (EditOrder)((Button)sender).CommandParameter;

                // Add the selected order to the OrderHistory collection
                await CrossCloudFirestore.Current
                    .Instance
                    .Collection("OrderHistory")
                    .AddAsync(orderhistory);

                // Delete the corresponding document from the PendingOrder collection
                if (orderhistory != null && !string.IsNullOrEmpty(orderhistory.OrderID))
                {
                    await CrossCloudFirestore.Current
                        .Instance
                        .Collection("PendingOrder")
                        .Document(orderhistory.OrderID)
                        .DeleteAsync();
                }
                else
                {
                    Console.WriteLine("Document ID is null or empty.");
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
            LoadPendingOrders();
        }
    }
}
