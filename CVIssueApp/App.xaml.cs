using CVIssueApp.Controls;
using System.Diagnostics;

namespace CVIssueApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CustomEditor.Setup();
            Utils.SetupLogFiles();
            AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("in OnStart().");
            Utils.WriteErrLog(null, msg: "App Start");
        }

        protected void HandleUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Utils.WriteErrLog(e, "(app-level error)");
        }
    }
}