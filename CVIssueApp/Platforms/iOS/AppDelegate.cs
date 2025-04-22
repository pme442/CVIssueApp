using Foundation;
using UIKit;

namespace CVIssueApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
        }

        [Export("applicationDidReceiveMemoryWarning:")]
        public void ReceiveMemoryWarning(UIApplication application)
        {
            ulong availableMemory = NSProcessInfo.ProcessInfo.PhysicalMemory;
            long totalMemory = System.GC.GetTotalMemory(false);
            Utils.WriteErrLog(null, msg: "totalMemory = " + totalMemory.ToString() + ", available = " + availableMemory.ToString());
        }

    }        
}
