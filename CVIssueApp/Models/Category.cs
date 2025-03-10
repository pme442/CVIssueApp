using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVIssueApp.Models
{
    public class Category : ObservableObject
    {
  
        public string CategoryPKey { get; set; }

        public string Name { get; set; }

        public ObservableCollection<Question> Questions { get; set; }


        private int _totalEligibleQuestions;
        public int TotalEligibleQuestions
        {
            get
            {
                if (Questions != null)
                {
                    //return Questions.Where(q => !q.Ineligible && !q.Hidden && q.Controltype != "labelonly").Count();
                    return Questions.Where(q => !q.Ineligible && q.Controltype != "labelonly" && q.Controltype != "postcalc").Count();
                }
                else
                {
                    return 0;
                }
            }
            set { _totalEligibleQuestions = value; OnPropertyChanged(nameof(TotalEligibleQuestions)); }

        }




        private int _totalAnsweredQuestions;
        public int TotalAnsweredQuestions
        {
            get
            {
                if (Questions != null)
                {
                    //return Questions.Count(q => q.Value != null && q.Value != "" && !q.Ineligible && !q.Hidden && q.Controltype != "labelonly");
                    return Questions.Count(q => q.Value != null && q.Value != "" && !q.Ineligible && q.Controltype != "labelonly" && q.Controltype != "postcalc");
                }
                else
                {
                    return 0;
                }
            }
            set { _totalAnsweredQuestions = value; OnPropertyChanged(nameof(TotalAnsweredQuestions)); }
        }



        public Category()
        {

        }
    }
}
