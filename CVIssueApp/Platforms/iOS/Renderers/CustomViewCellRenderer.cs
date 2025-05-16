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
            base.DisconnectHandler(platformView);
        }


    }
}
