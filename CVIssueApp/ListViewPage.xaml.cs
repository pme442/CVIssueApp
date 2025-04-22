namespace CVIssueApp;

public partial class ListViewPage : ContentPage
{
    public ListViewPageViewModel vm;

    public ListViewPage()
	{
        InitializeComponent();
        Content.BindingContext = vm = new ListViewPageViewModel();
    }

    private async void CategoryListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (vm.IsLoaded)
        {
            await vm.OnItemTapped(e.SelectedItem);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!vm.IsLoaded)
        {
            vm.OnAppearing();
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // this never gets called
        base.OnNavigatedTo(args);
        vm.OnAppearing();
    }

}