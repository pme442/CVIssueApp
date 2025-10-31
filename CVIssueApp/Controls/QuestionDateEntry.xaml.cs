using System.Diagnostics;
using System.Windows.Input;
using System.Reflection;
using CVIssueApp.Models;

namespace CVIssueApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionDateEntry : BaseQuestionEntryTemplate
    {
        private bool isLoaded = false;

        // In Error
        public static readonly BindableProperty InErrorProperty = BindableProperty.Create("InError", typeof(bool), typeof(QuestionDateEntry), false, BindingMode.Default, propertyChanged: OnInErrorPropertyChanged);
        public bool InError
        {
            get { return (bool)GetValue(InErrorProperty); }
            set { SetValue(InErrorProperty, value); }
        }

        private static void OnInErrorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (QuestionDateEntry)bindable;
            control.InError = (bool)newValue;
        }

        // Error Message
        public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create("ErrorMessage", typeof(string), typeof(QuestionDateEntry), string.Empty, BindingMode.Default, propertyChanged: OnErrorMessagePropertyChanged);
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        private static void OnErrorMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (QuestionDateEntry)bindable;
            control.ErrorMessage = newValue.ToString();
        }

  
        public QuestionDateEntry()
        {
            ErrorMessage = "";
            InitializeComponent();
        }

        public object GetNewQuestionValue(object val)
        {
            var result = val;

            if (result is null)
            {
                result = "";
            }
            return result;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {

            if (BindingContext is null || !(sender is DatePicker datepicker))
            {
                return;
            }
            if (!(datepicker.BindingContext is Question))
            {
                return;
            }

            if (e.OldDate != e.NewDate) // if the dates are the same, no need to do anything
            {
                SaveValue(BindingContext, e.NewDate);
            }
        }

        public void SaveNewValue(object newDate)
        {

            if (this.BindingContext is Question theQuest)
            {
                if (newDate != null)
                {
                    DateTime dtObjnewDateTime = (DateTime)newDate;
                    string strnewValue = dtObjnewDateTime.ToString("MM/dd/yyyy");

                    if (strnewValue != theQuest.Value)
                    {
                        SaveValue(this.BindingContext, (DateTime?)newDate);
                    }
                }
                else
                {
                    SaveValue(this.BindingContext, (DateTime?)newDate);
                }
            }
        }

        public async void SaveValue(object theQuestionObj, object newVal)
        {

            MethodInfo theSaveMethod = ParentBindingContext.GetType().GetMethod("OnQuestionChanged");
            if (theSaveMethod != null)
            {
                object theParentBindingContext = ParentBindingContext;
                try
                {
                    object theNewVal = GetNewQuestionValue(newVal);
                    var task = (Task<QuestionUpdateResult>)theSaveMethod.Invoke(ParentBindingContext, new object[] { theQuestionObj, theNewVal });
                    QuestionUpdateResult result = await task;

                 
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    theParentBindingContext = null;
                }
            }
        }






    }
}
