using NiisMenu.Builder;
using Plugin.CloudFirestore;

namespace NiisMenu;

public partial class ListMenu : ContentPage
{
	public ListMenu()
	{
		InitializeComponent();
        CategoryPicker.SelectedIndexChanged += async (sender, e) => await LoadMenuItem();
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

    private async Task LoadMenuItem()
    {
        List<Menu> memuItems = await GetMenuItemsAsync();
        MenuListView.ItemsSource = memuItems;
    }


    public async Task<List<Menu>> GetMenuItemsAsync()
    {
        List<Menu> menuItems = new List<Menu>();

        if (CategoryPicker.SelectedItem != null)
        {
            var querySnapshot = await CrossCloudFirestore.Current
                                                          .Instance
                                                          .Collection("Menu")
                                                          .WhereEqualsTo("Category", CategoryPicker.SelectedItem.ToString())
                                                          .GetAsync();
            foreach (var document in querySnapshot.Documents)
            {
                Menu menu = document.ToObject<Menu>();
                menuItems.Add(menu);
            }

        }
        else
        {
            var querySnapshot = await CrossCloudFirestore.Current
                                                      .Instance
                                                      .Collection("Menu")
                                                      .GetAsync();
            foreach (var document in querySnapshot.Documents)
            {
                Menu menu = document.ToObject<Menu>();
                menuItems.Add(menu);
            }
        }
        

        return menuItems;
    }

    private bool isbuttonEnabled = true;
    private void MenuListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (!isbuttonEnabled)
        {
            return;
        }
        isbuttonEnabled = false;
        try 
        {

            var selectedMenu = (Menu)e.SelectedItem;
            Navigation.PushAsync(new UpdateMenu(selectedMenu));
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            isbuttonEnabled = true;
        }
    }
}