using CVIssueApp.Controls;

namespace CVIssueApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CustomEditor.Setup();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}