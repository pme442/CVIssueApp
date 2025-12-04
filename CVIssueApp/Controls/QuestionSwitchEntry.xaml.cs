using System.Diagnostics;
using System.Reflection;
using CVIssueApp.Models;

namespace CVIssueApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionSwitchEntry : BaseQuestionEntryTemplate
    {

        //public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(QuestionSwitchEntry), string.Empty, BindingMode.OneWay, propertyChanged: OnValuePropertyChanged);
        //public string Value
        //{
        //    get { return (string)GetValue(ValueProperty); }
        //    set { SetValue(ValueProperty, value); }
        //}

        //private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    if (newValue != oldValue)
        //    {
        //        var control = (QuestionSwitchEntry)bindable;
        //        control.Value = newValue == null ? "" : newValue.ToString();
        //    }
        //}
    
        public bool ToggledIsBusy = false;

      
        public QuestionSwitchEntry()
        {
            InitializeComponent();
        }

        public string GetNewQuestionValue(bool toggledOn, string theOptionValue)
        {
            string result = "";

            if (toggledOn)
            {
                result = theOptionValue;
            }
            
            return result;
        }


        private async void OnSwitchToggled(object sender, ToggledEventArgs e)
        {

            // Known bug: XAML Switch Toggled event behaviour OnAppearing and Toggled not making sense. #9099
            // When a true switch is set to false, Toggled event is fired once.
            // When a false switch is set to true Toggled event is fired twice (once for the change and then once again for each item in the list as the form is redrawn).

            if (ToggledIsBusy || BindingContext is null || !(sender is Microsoft.Maui.Controls.Switch thisSwitch))
            {
                return;
            }
            if (!(thisSwitch.BindingContext is QuestionOption))
            {
                return;
            }

            try
            {
                var question = (Question)BindingContext;
                var questionOption = (QuestionOption)thisSwitch.BindingContext;
                var newVal = GetNewQuestionValue(e.Value, questionOption.Value);
                //var oldVal = Value == null ? "" : Value;
                var oldVal = question.Value == null ? "" : question.Value;

                if (newVal != oldVal)
                {                    
                    // If a switch was turned on (and a different switch is already turned on), turn off any other in the group.
                    if (e.Value && (oldVal != "" && oldVal != null))
                    {
                        bool stopLoop = false;
                        List<IView> GridList = SwitchGroup.Children.Where(x => x is Grid).ToList();
                        foreach (Grid gridObj in GridList.Cast<Grid>())
                        {
                            if (stopLoop)
                            {
                                break;
                            }
                            foreach (Microsoft.Maui.Controls.Switch switchObj in gridObj.Children.OfType<Microsoft.Maui.Controls.Switch>())
                            {
                                if (switchObj != thisSwitch && switchObj.IsToggled)
                                {
                                    ToggledIsBusy = true;
                                    //switchObj.Toggled -= OnSwitchToggled;
                                    switchObj.IsToggled = false;
                                    ToggledIsBusy = false;
                                    //switchObj.Toggled += OnSwitchToggled;

                                    stopLoop = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (ParentBindingContext != null)
                    {
                        PropertyInfo isBusyProperty = ParentBindingContext.GetType().GetProperty("IsBusy");
                        if (isBusyProperty != null)
                        {
                            if (!(bool)isBusyProperty.GetValue(ParentBindingContext))
                            {
                                await SaveValue(question, newVal);
                            }
                            else
                            {
                                ToggledIsBusy = true;
                                //thisSwitch.Toggled -= OnSwitchToggled;
                                thisSwitch.IsToggled = !e.Value;
                                ToggledIsBusy = false;
                                //thisSwitch.Toggled += OnSwitchToggled;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task SaveValue(object theQuestionObj, object newVal)
        {

            MethodInfo theSaveMethod = ParentBindingContext.GetType().GetMethod("OnQuestionChanged");
            if (theSaveMethod != null)
            {
                object theParentBindingContext = ParentBindingContext;
                try
                {
                    var task = (Task<QuestionUpdateResult>)theSaveMethod.Invoke(ParentBindingContext, new object[] { theQuestionObj, newVal });
                    QuestionUpdateResult result = await task;

                    if (task.IsFaulted)
                    {
                        Debug.WriteLine("QuestionSwitchEntry task.IsFaulted");
                    }
                    else if (!task.IsCompleted)
                    {
                        Debug.WriteLine("QuestionSwitchEntry !task.IsCompleted");
                    }
                    else
                    {
                       // do nothing
                    }
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