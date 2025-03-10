
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Diagnostics;



namespace CVIssueApp
{
    [Serializable]
    //public class ObservableObject : INotifyPropertyChanged, INotifyCollectionChanged, IDisposable
    public class ObservableObject : INotifyPropertyChanged, INotifyCollectionChanged
    {
        readonly Dictionary<string, List<string>> _propertyDependencies = new Dictionary<string, List<string>>();
        readonly Dictionary<string, List<Action<ObservableObject, string>>> _propertyActions = new Dictionary<string, List<Action<ObservableObject, string>>>();
        readonly Dictionary<string, List<Func<Task>>> _propertyTasks = new Dictionary<string, List<Func<Task>>>();

        public ObservableObject()
        {
            _propertyDependencies = new Dictionary<string, List<string>>();
            _propertyActions = new Dictionary<string, List<Action<ObservableObject, string>>>();
            _propertyTasks = new Dictionary<string, List<Func<Task>>>();

            PropertyChanged += OnPropertyChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                //if (sender.GetType().IsSubclassOf(typeof(ViewModels.BaseViewModel))) 
                //{

                //    ViewModels.BaseViewModel basevm = (ViewModels.BaseViewModel)sender;
                //    Debug.WriteLine("OnPropertyChanged, vm = " + basevm.GetType() + ", property = " + e.PropertyName);
                //    if (basevm.IsLoaded)
                //    {                
                //        basevm.PageHasChanges = true;
                //    }
                //}                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnCollectionChanged<T>(Expression<Func<T>> e)
        {
            try
            {
                var handler = this.CollectionChanged;
                handler?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }






    }
}
