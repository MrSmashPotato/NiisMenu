using NiisMenu.Builder;
using Plugin.CloudFirestore;

namespace NiisMenu;

public partial class ListMenu : ContentPage
{
	public ListMenu()
	{
		InitializeComponent();
        MessagingCenter.Subscribe<UpdateMenu>(this, "DataUpdated", (sender) =>
        {
            // Call the method to reload the data
            LoadMenuItem();
        });
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMenuItem();
    }

    private async void LoadMenuItem()
    {
        List<Menu> memuItems = await GetMenuItemsAsync();
        MenuListView.ItemsSource = memuItems;
    }


    public async Task<List<Menu>> GetMenuItemsAsync()
    {
        List<Menu> menuItems = new List<Menu>();

        var querySnapshot = await CrossCloudFirestore.Current
                                                      .Instance
                                                      .Collection("Menu")
                                                      
                                                      .GetAsync();

        foreach (var document in querySnapshot.Documents)
        {
            Menu menu = document.ToObject<Menu>();
            menuItems.Add(menu);
        }

        return menuItems;
    }

    private void MenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedMenu = (Menu)e.SelectedItem;
        Navigation.PushAsync(new UpdateMenu(selectedMenu));
    }
}