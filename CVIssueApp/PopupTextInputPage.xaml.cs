using Mopups.Pages;
using Mopups.Services;
using System.Diagnostics;

namespace CVIssueApp;

[XamlCompilation(XamlCompilationOptions.Compile)]

public partial class PopupTextInputPage : PopupPage
{

    public PopupTextInputViewModel vm;
    public PopupTextInputPage(string LabelText, string Text, object DataObject, Action<string> OnClickDone) : base()
    {
        this.HasSystemPadding = false;
        this.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.4f);
        InitializeComponent();
        Content.BindingContext = vm = new PopupTextInputViewModel(LabelText, Text, DataObject, OnClickDone);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
                        
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    protected override bool OnBackgroundClicked()
    {
        return false;
    }

    private void TheCustomEditor_Completed(object sender, EventArgs e)
    {
        vm.ClickDoneBtnCommand.Execute(null);
    }
}