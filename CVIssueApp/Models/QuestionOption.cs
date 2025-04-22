using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVIssueApp.Models
{
    public class QuestionOption : ObservableObject
    {
        public string QuestionOptionPKey { get; set; }

        public Question Question { get; set; }

        public string QuestionPKey { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                bool isSel = false;
                string? val = null;
                string controlType = "";

                if (Question != null)
                {
                    val = Question.Value;
                    controlType = Question.Controltype;
                }               
                if (val != null)
                {
                    if (val.ToLower() == Value.ToLower())
                    {
                        isSel = true;
                    }
                }
                return isSel;
            }

            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }

        }
    }
}
