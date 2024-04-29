using NiisMenu.Builder;
using Plugin.CloudFirestore;

namespace NiisMenu;

public partial class CustomerPage : ContentPage
{
	public CustomerPage()
	{
		InitializeComponent();
        CategoryPicker.SelectedIndexChanged += (sender, e) => LoadMenuItem();
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
                                                      .WhereEqualsTo("Category",CategoryPicker.SelectedItem)
                                                      .GetAsync();

        foreach (var document in querySnapshot.Documents)
        {
            Menu menu = document.ToObject<Menu>();
            menuItems.Add(menu);
        }

        return menuItems;
    }

    private void Refresh_Clicked(object sender, EventArgs e)
    {
        LoadMenuItem();

    }
}