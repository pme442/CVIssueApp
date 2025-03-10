using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVIssueApp.Controls
{

    public class CustomViewCell : ViewCell, INotifyPropertyChanged
    {
        public static readonly BindableProperty SelectedItemBackgroundColorProperty =
        BindableProperty.Create("SelectedItemBackgroundColor",
                                typeof(Color),
                                typeof(CustomViewCell),
                                null);

        public Color SelectedItemBackgroundColor
        {
            get { return (Color)GetValue(SelectedItemBackgroundColorProperty); }
            set { SetValue(SelectedItemBackgroundColorProperty, value); OnPropertyChanged(nameof(SelectedItemBackgroundColor)); }
        }

        public CustomViewCell() : base()
        {
            SelectedItemBackgroundColor = (Color)App.Current.Resources["ColorSelectedRow"];
        }

        //protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        //{
        //    child.Parent = null;
        //    base.OnChildRemoved(child, oldLogicalIndex);
        //}
        //protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    var thePropName = propertyName;
        //    //if (thePropName == "Window")
        //    //{
        //    //    if (this.View.Window == null)
        //    //    {
        //    //        Debug.WriteLine("window is null");// ^anc_
        //    //    }
        //    //}
        //    base.OnPropertyChanged(propertyName);
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //}
    }
    
}
