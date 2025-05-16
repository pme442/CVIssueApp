using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Reflection;

namespace CVIssueApp.Controls
{
    public class BaseQuestionEntryTemplateLV : CustomViewCell, IDisposable
    {
        public static BindableProperty ParentBindingContextProperty = BindableProperty.Create(nameof(ParentBindingContext), typeof(object), typeof(BaseQuestionEntryTemplateLV), null);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set { SetValue(ParentBindingContextProperty, value); }
        }

        public static readonly BindableProperty ContextMenuTapCommandProperty = BindableProperty.Create("ContextMenuTapCommand", typeof(ICommand), typeof(BaseQuestionEntryTemplateLV), null);

        public ICommand ContextMenuTapCommand
        {
            get { return (ICommand)GetValue(ContextMenuTapCommandProperty); }
            set { SetValue(ContextMenuTapCommandProperty, value); }
        }

        public Func<object, bool> AllowPopup => ShouldAllowPopup;

        public bool ShouldAllowPopup(object arg)
        {
            // Add logic to determine if the UI should be disabled 
            // while executing a task that is observing this action.  
            return CanAllowPopup;
        }

        private bool canAllowPopup = true;
        public bool CanAllowPopup
        {
            get { return canAllowPopup; }
            set
            {
                canAllowPopup = value;
                OnPropertyChanged(nameof(CanAllowPopup));
                RefreshCommandCanExecutes();
            }
        }

        protected virtual void RefreshCommandCanExecutes()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ((Command)ContextMenuTapCommand).ChangeCanExecute();
            });
        }

        protected async Task OnContextMenuTapCommand(object sender)
        {
            if (!CanAllowPopup)
            {
                return;
            }

            CanAllowPopup = false;

            await OpenContextMenu(sender);

            CanAllowPopup = true;
        }

        protected virtual async Task OpenContextMenu(object sender)
        {
            await Task.Delay(0);
        }

        public BaseQuestionEntryTemplateLV() : base()
        {
            ContextMenuTapCommand = new Command(async (x) => await OnContextMenuTapCommand(x), ShouldAllowPopup);

        }

        public void OnAnsweredByTextChanged(object sender, Microsoft.Maui.Controls.PropertyChangingEventArgs e)
        {
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS && e.PropertyName == "Text")
            {
                if (sender is Label theLabel)
                {
                    if (theLabel.BindingContext is Models.Question theQuestion)
                    {
                        if (string.IsNullOrEmpty(theLabel.Text) && string.IsNullOrEmpty(theQuestion.AnsweredByText))
                        {
                            return;
                        }
                        else
                        {
                            if (theLabel.Text == theQuestion.AnsweredByText)
                            {
                                return;
                            }
                        }

                        if (this.Parent is CustomListView theListview)
                        {
                            theListview.ForceNativeTableUpdate(true);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            ContextMenuTapCommand = null;
            ParentBindingContext = null;

            if (this is Controls.QuestionTextEntryLV questTextEntry)
            {
                questTextEntry.LastValueTapCommand = null;
            }
        }
    }

    public class QuestionEntryTemplateSelectorLV : DataTemplateSelector
    {

        public DataTemplate QuestionTextTemplate { get; set; }
        public DataTemplate QuestionSwitchTemplate { get; set; }
        public DataTemplate BlankTemplate { get; set; }


        public QuestionEntryTemplateSelectorLV()
        {
            //Retain instances!
            QuestionTextTemplate = new DataTemplate(typeof(QuestionTextEntryLV));
            QuestionSwitchTemplate = new DataTemplate(typeof(QuestionSwitchEntryLV));
            BlankTemplate = new DataTemplate(typeof(QuestionHiddenEntryLV));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            if (item == null)
            {
                return BlankTemplate;
            }

            Type theItemType = item.GetType();
            PropertyInfo ControltypeProperty = theItemType.GetProperty("Controltype");
            if (ControltypeProperty is null)
            {
                return BlankTemplate;
            }
            PropertyInfo VisibleProperty = theItemType.GetProperty("Visible");
            if (VisibleProperty != null)
            {
                if (VisibleProperty.GetValue(item) is bool visibleValue)
                {
                    if (!visibleValue)
                    {
                        return BlankTemplate;
                    }
                }
            }


            switch (ControltypeProperty.GetValue(item))
            {
                case "text":
                case "textbox":
                    QuestionTextTemplate.SetValue(BaseQuestionEntryTemplateLV.ParentBindingContextProperty, container.BindingContext);
                    return QuestionTextTemplate;
                case "switch":
                    QuestionSwitchTemplate.SetValue(BaseQuestionEntryTemplateLV.ParentBindingContextProperty, container.BindingContext);
                    return QuestionSwitchTemplate;
                default:
                    // text                    
                    QuestionTextTemplate.SetValue(BaseQuestionEntryTemplateLV.ParentBindingContextProperty, container.BindingContext);
                    return QuestionTextTemplate;
            }





        }

    }


}
