using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
using CVIssueApp.Controls;
using UIKit;

namespace CVIssueApp.Platforms.iOS.Renderers
{
    public partial class CustomViewCellRenderer : ViewCellRenderer
    {

        // Currently, the property mapper doesn't work for viewcellrenderer, so we're setting everything in CreatePlatformElement().
        // Known issue: ViewCellRenderer Issues #14079

        /// <summary>
        /// The Property mapper.
        /// </summary>
        public static IPropertyMapper<CustomViewCell, CustomViewCellRenderer> CustomViewCellMapper
            = new PropertyMapper<CustomViewCell, CustomViewCellRenderer>(Mapper)
            {
                [nameof(CustomViewCell.SelectedItemBackgroundColor)] = MapColor
            };

        public CustomViewCellRenderer()
        {

        }

        protected override UITableViewCell CreatePlatformElement()
        {
            CustomViewCell cvcell = (CustomViewCell)VirtualView;
            UITableViewCell uitbvcell = base.CreatePlatformElement();
            uitbvcell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = cvcell.SelectedItemBackgroundColor.ToPlatform()
            };

            // 2-18-25 anc
            // If you have a FormMultiSelectEntry in a listview and you select a bunch of items to make the combobox height increase, it would cover up the control(s) under it.
            // See QuestionAlarmCommentImagesPage: Problems, Codes, Actions.
            // It would only resize if you close the page and reopen or scroll it off the screen and then back into view.
            // This PropertyChanged event is a workaround until the bug is fixed.
            // Known issue: [iOS] ListView with HasUnevenRows="true" doesn't resize dynamically #15053
            //              ListView with resizing ViewCell has strange behaviour on iOS #8239
            //              [iOS] ViewCell not resized when HasUnevenRows is enabled and cell content changes #23319
            cvcell.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SelectedItems")
                {
                    cvcell.ForceUpdateSize();
                    if (cvcell.Parent is ListView lv)
                    {
                        lv.HasUnevenRows = false;
                        lv.HasUnevenRows = true;
                    }
                }
            };

            return uitbvcell;

            //return base.CreatePlatformElement();
        }

        private static void MapColor(CustomViewCellRenderer handler, CustomViewCell view)
        {
            handler.PlatformView.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedItemBackgroundColor.ToPlatform(),
            };
        }

        protected override void ConnectHandler(UITableViewCell platformView)
        {
            base.ConnectHandler(platformView);

        }

        protected override void DisconnectHandler(UITableViewCell platformView)
        {
            //var z = VirtualView as IVisualTreeElement;
            //MemoryToolkit.Utilities.TearDown(z);

            //var y = VirtualView.GetVisualTreeDescendants();
            //var x = platformView.Subviews;

            base.DisconnectHandler(platformView);
        }


    }
}
