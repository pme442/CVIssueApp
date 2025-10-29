namespace CVIssueApp;

public partial class TestPage : ContentPage
{
    public TestPageViewModel vm;

    public TestPage()
	{
		InitializeComponent();
        Content.BindingContext = vm = new TestPageViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!vm.IsLoaded)
        {
            vm.OnAppearing();
        }
    }


}