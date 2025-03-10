using CVIssueApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVIssueApp.Controls
{
    public class CustomListView : ListView, IDisposable
    {
        // 8-5-24 anc
        // In an attempt to improve performance and memory usage, I discovered the following (for iOS): 
        // 1. RecycleElementAndDataTemplate does not work if you are using a single ViewModel/Model foreach template.
        // See issue: [Bug] [iOS] [Android] ListView bug for CachingStrategy="RecycleElementAndDataTemplate" #9998
        // 2. RecycleElement causes this issue:  If an item height changes after it is initially drawn (e.g. "Comment is required" message appears), if you then try to open the action menu, the app freezes.
        public CustomListView() : this(DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android ? ListViewCachingStrategy.RetainElement : ListViewCachingStrategy.RecycleElement)
        {
            this.ItemSelected += CustomListView_ItemSelected;
        }

        // 4-22-21 anc
        // Workaround for: [iOS] Xamarin.Forms Listview Row Height Does Not Update When Changing Content Size (such as label) #2383
        // See also: CustomListViewRenderer_iOS
        public void ForceNativeTableUpdate(bool useDelay = false)
        {
            ViewCellSizeChangedEvent?.Invoke(useDelay);
        }

        public void Dispose()
        {
            this.ItemSelected -= CustomListView_ItemSelected;
        }


        public event Action<bool> ViewCellSizeChangedEvent;

        private void CustomListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CustomListView theListView = (CustomListView)sender;
            if (theListView.IsMultiSelect)
            {
                return;
            }

            if (theListView.ScrollToRowWhenSelected)
            {
                if (e.SelectedItem != null)
                {
                    if (e.SelectedItem is Question)
                    {                    
                        theListView.ScrollTo(e.SelectedItem, ScrollToPosition.MakeVisible, false);
                        theListView.ScrollToRowWhenSelected = false;
                    }
                    else
                    {
                        
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            theListView.ScrollTo(e.SelectedItem, ScrollToPosition.MakeVisible, false);
                        });
                        
                        theListView.ScrollToRowWhenSelected = false;
                    }
                }
            }

        }

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            IsPullToRefreshEnabled = false;
        }


        public static readonly BindableProperty TotalRowsProperty =
            BindableProperty.CreateAttached(nameof(TotalRows), typeof(int), typeof(CustomListView), 0, BindingMode.TwoWay);
        public int TotalRows
        {
            get { return (int)GetValue(TotalRowsProperty); }
            set { SetValue(TotalRowsProperty, value); }
        }

        public static readonly BindableProperty ColTotalsProperty =
            BindableProperty.CreateAttached(nameof(ColTotals), typeof(int), typeof(CustomListView), 0, BindingMode.TwoWay);
        public int ColTotals
        {
            get { return (int)GetValue(ColTotalsProperty); }
            set { SetValue(ColTotalsProperty, value); }
        }


        public static readonly BindableProperty IsMultiSelectProperty =
            BindableProperty.CreateAttached("IsMultiSelect", typeof(bool), typeof(CustomListView), false, BindingMode.TwoWay);
        public bool IsMultiSelect
        {
            get { return (bool)GetValue(IsMultiSelectProperty); }
            set { SetValue(IsMultiSelectProperty, value); OnPropertyChanged(nameof(IsMultiSelect)); }
        }

        public static readonly BindableProperty IsMainCheckedProperty =
            BindableProperty.CreateAttached("IsMainChecked", typeof(bool), typeof(CustomListView), false);
        public bool IsMainChecked
        {
            get { return (bool)GetValue(IsMainCheckedProperty); }
            set { SetValue(IsMainCheckedProperty, value); }
        }

        public static readonly BindableProperty IsExpandableProperty =
            BindableProperty.CreateAttached("IsExpandable", typeof(bool), typeof(CustomListView), false);
        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }

        public static readonly BindableProperty IsExpandedProperty =
            BindableProperty.CreateAttached("IsExpanded", typeof(bool), typeof(CustomListView), false, BindingMode.TwoWay);
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly BindableProperty IsEarlyFilterableProperty =
            BindableProperty.CreateAttached("IsEarlyFilterable", typeof(bool), typeof(CustomListView), false);
        public bool IsEarlyFilterable
        {
            get { return (bool)GetValue(IsEarlyFilterableProperty); }
            set { SetValue(IsEarlyFilterableProperty, value); }
        }

        public static readonly BindableProperty IsEarlyFilterOnProperty =
            BindableProperty.CreateAttached("IsEarlyFilterOn", typeof(bool), typeof(CustomListView), false, BindingMode.TwoWay);
        public bool IsEarlyFilterOn
        {
            get { return (bool)GetValue(IsEarlyFilterOnProperty); }
            set { SetValue(IsEarlyFilterOnProperty, value); }
        }

        public static readonly BindableProperty CanDeleteProperty =
            BindableProperty.CreateAttached("CanDelete", typeof(bool), typeof(CustomListView), true, BindingMode.TwoWay);
        //propertyChanged: (bindable, oldValue, newValue) => ((CustomListView)bindable).CanDeleteChanged());
        public bool CanDelete
        {
            get { return (bool)GetValue(CanDeleteProperty); }
            set { SetValue(CanDeleteProperty, value); }
        }

        public static readonly BindableProperty ScrollToRowWhenSelectedProperty =
            BindableProperty.CreateAttached("ScrollToRowWhenSelected", typeof(bool), typeof(CustomListView), false);
        public bool ScrollToRowWhenSelected
        {
            get { return (bool)GetValue(ScrollToRowWhenSelectedProperty); }
            set { SetValue(ScrollToRowWhenSelectedProperty, value); }
        }

        public static BindableProperty ItemTappedCommandProperty =
            BindableProperty.CreateAttached("ItemTappedCommand", typeof(ICommand), typeof(ListView), null);

        public static ICommand GetItemTappedCommand(BindableObject view) => (ICommand)view.GetValue(ItemTappedCommandProperty);

        public static void SetItemTappedCommand(BindableObject view, ICommand value) => view.SetValue(ItemTappedCommandProperty, value);


        public static readonly BindableProperty SortColumnProperty =
            BindableProperty.CreateAttached("SortColumn", typeof(string), typeof(CustomListView), string.Empty, BindingMode.TwoWay);
        public string SortColumn
        {
            get { return (string)GetValue(SortColumnProperty); }
            set { SetValue(SortColumnProperty, value); }
        }

        public static readonly BindableProperty SortDirectionProperty =
            BindableProperty.CreateAttached("SortDirection", typeof(string), typeof(CustomListView), string.Empty, BindingMode.TwoWay);
        public string SortDirection
        {
            get { return (string)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        public static readonly BindableProperty HighlightSelectionProperty =
            BindableProperty.CreateAttached("HighlightSelection", typeof(bool), typeof(CustomListView), true);
        public bool HighlightSelection
        {
            get { return (bool)GetValue(HighlightSelectionProperty); }
            set { SetValue(HighlightSelectionProperty, value); }
        }

        public static readonly BindableProperty EnableDeleteProperty =
            BindableProperty.CreateAttached("EnableDelete", typeof(bool), typeof(CustomListView), false);
        public bool EnableDelete
        {
            get { return (bool)GetValue(EnableDeleteProperty); }
            set { SetValue(EnableDeleteProperty, value); }
        }

        public static readonly BindableProperty EnableShowDetailsProperty =
            BindableProperty.CreateAttached("EnableShowDetails", typeof(bool), typeof(CustomListView), false);
        public bool EnableShowDetails
        {
            get { return (bool)GetValue(EnableShowDetailsProperty); }
            set { SetValue(EnableShowDetailsProperty, value); }
        }

        public static readonly BindableProperty EnableShowEarlyProperty =
            BindableProperty.CreateAttached("EnableShowEarly", typeof(bool), typeof(CustomListView), false);
        public bool EnableShowEarly
        {
            get { return (bool)GetValue(EnableShowEarlyProperty); }
            set { SetValue(EnableShowEarlyProperty, value); }
        }

        public static readonly BindableProperty ShowRefreshProperty =
            BindableProperty.CreateAttached("ShowRefresh", typeof(bool), typeof(CustomListView), false);
        public bool ShowRefresh
        {
            get { return (bool)GetValue(ShowRefreshProperty); }
            set { SetValue(ShowRefreshProperty, value); }
        }







    }

}
