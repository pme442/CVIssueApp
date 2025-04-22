namespace CVIssueApp;

public partial class ErrorLogPage : ContentPage
{
    public ErrorLogPageViewModel vm;

    public ErrorLogPage()
	{
		InitializeComponent();
        Content.BindingContext = vm = new ErrorLogPageViewModel();
    }

    private async void Label_SizeChanged(object sender, EventArgs e)
    {
        var theLabel = (Label)sender;
        var theScrollView = (ScrollView)theLabel.Parent;

        await theScrollView.ScrollToAsync(0, theScrollView.Content.Height, false);
    }

    protected override void OnAppearing()
    {
        ulong availableMemory = Foundation.NSProcessInfo.ProcessInfo.PhysicalMemory;
        long totalMemory = System.GC.GetTotalMemory(false);
        Utils.WriteErrLog(null, msg: "totalMemory = " + totalMemory.ToString() + ", available = " + availableMemory.ToString());

        base.OnAppearing();
        vm.OnAppearing();        
    }
}