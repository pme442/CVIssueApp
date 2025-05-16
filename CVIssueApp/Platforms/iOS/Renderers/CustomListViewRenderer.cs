using CoreFoundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using CVIssueApp.Controls;
using UIKit;

namespace CVIssueApp.Platforms.iOS.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer //iOS
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement is CustomListView customListView)
            {
                customListView.ViewCellSizeChangedEvent += UpdateTableView;
            }
        }

        private void UpdateTableView(bool useDelay = false)
        {
            if (useDelay)
            {
                DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(0.2)), () => { if (!(Control is UITableView tv)) { return; } tv.BeginUpdates(); tv.EndUpdates(); });
            }
            else
            {
                if (!(Control is UITableView tv)) return;
                tv.BeginUpdates();
                tv.EndUpdates();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Element != null)
                {
                    if (Element is CustomListView customListView)
                    {
                        customListView.ViewCellSizeChangedEvent -= UpdateTableView;
                    }
                }
            }
            base.Dispose(disposing);
        }


    }

}
