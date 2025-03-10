using CVIssueApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CVIssueApp
{
    public static class Utils
    {

        public static void ShowTextInput(string LabelText, object TextValue, Object DataObject, Action<string> callback, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerName = null)
        {
            try
            {
                if (TextValue is null)
                {
                    TextValue = string.Empty;
                }

                var inputPage = new PopupTextInputPage(LabelText, TextValue.ToString(), DataObject, callback);

                // Make sure this window is not already open
                if (Mopups.Services.MopupService.Instance.PopupStack.Count == 0 || !(Mopups.Services.MopupService.Instance.PopupStack[(Mopups.Services.MopupService.Instance.PopupStack.Count - 1)].GetType() == typeof(PopupTextInputPage)))
                {
                    //await Mopups.Services.MopupService.Instance.PushAsync(inputPage, false);
                    App.Current.Dispatcher.DispatchAsync(async () => { await Mopups.Services.MopupService.Instance.PushAsync(inputPage, false); });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public struct TextValidationResult
        {
            public bool InError;
            public string ErrorMessage;
            public string NewTextValue;
            public Color RequiredIndColor;
        }

        public static TextValidationResult ValidateQuestionTextInput(string textValue, Question q)
        {
            TextValidationResult vResult = new TextValidationResult() { InError = false, ErrorMessage = "", NewTextValue = textValue, RequiredIndColor = Colors.Transparent };

            try
            {
                if (textValue == null)
                {
                    return vResult;
                }

                string newTextValue = string.Empty;

                // remove line breaks
                newTextValue = Utils.RemoveLineBreaks(textValue);
              
                vResult.NewTextValue = newTextValue;
                vResult.InError = false;
                vResult.ErrorMessage = string.Empty;
                            
                return vResult;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return vResult;
            }

        }

        public static string RemoveLineBreaks(string s)
        {
            return System.Text.RegularExpressions.Regex.Replace(s, @"\t|\n|\r", string.Empty);
        }


    }
}
