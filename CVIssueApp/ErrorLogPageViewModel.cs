using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CVIssueApp
{
    public class ErrorLogPageViewModel : ObservableObject
    {
        private string logText;
        public string LogText
        {
            get { return logText; }
            set
            {
                logText = value;
                OnPropertyChanged(nameof(LogText));
            }
        }

        private ICommand _clickCommandButton;
        public ICommand ClickCommandButton
        {
            get { return _clickCommandButton ?? (_clickCommandButton = new Command( async () => await ClearLog())); }
        }

        public void OnAppearing()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            try
            {
                LogText = Utils.LoadFromFile();                
            }
            catch (Exception ex)
            {
                Debug.WriteLine( ex);
            }

        }

        public async Task ClearLog()
        {
            try
            {
                await Utils.PurgeErrorLog();
                RefreshData();                
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex);
            }

        }
    }
}
