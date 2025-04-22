using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVIssueApp;
using System.Collections.ObjectModel;
using CVIssueApp.Models;
using System.Diagnostics;
using System.Windows.Input;

namespace CVIssueApp
{    
    public class MainPageViewModel : ObservableObject
    {
        private bool _isLoaded;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        private ObservableCollectionFast<Category> categories;
        public ObservableCollectionFast<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Category));

            }
        }

        private bool enableCategories;
        public bool EnableCategories
        {
            get { return enableCategories; }
            set
            {
                enableCategories = value;
                OnPropertyChanged(nameof(EnableCategories));
            }
        }

        private Category selectedCategory;
        public Category SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (value != SelectedCategory)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }



        private ObservableCollectionFast<QuestionGrouping> questionsGrouped;
        public ObservableCollectionFast<QuestionGrouping> QuestionsGrouped
        {
            get { return questionsGrouped; }
            set
            {
                questionsGrouped = value;
                OnPropertyChanged(nameof(QuestionsGrouped));
            }
        }

        private bool questionsVisible;
        public bool QuestionsVisible
        {
            get { return questionsVisible; }
            set
            {
                questionsVisible = value;
                OnPropertyChanged(nameof(QuestionsVisible));
            }
        }

        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                if (value != SelectedQuestion)
                {
                    selectedQuestion = value;
                    OnPropertyChanged(nameof(SelectedQuestion));
                }
            }
        }

        public string SelectedCategoryPKey { get; set; }

        private ICommand _clickCommandButton;
        public ICommand ClickCommandButton
        {
            get { return _clickCommandButton ?? (_clickCommandButton = new Command(async (x) => await OnClicked(x))); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        ObservableCollection<Question> tempQuestions = new ObservableCollection<Question>();
        List<QuestionGrouping> tempQuestionsGrouped = new List<QuestionGrouping>();
        IEnumerable<QuestionGrouping> sortedQuestions;
        List<Category> tempCategories = new List<Category>();
        private string LastSelectedCategoryPKey = "";
        private bool ForceQuestionReload = false;
        private bool LoadingQuestions = false;        


        public MainPageViewModel()
        {
            Categories = new ObservableCollectionFast<Category>();
            QuestionsGrouped = new ObservableCollectionFast<QuestionGrouping>();            
            EnableCategories = false;
            QuestionsVisible = true;
        }

        public async void OnAppearing()
        {

            await Task.Run(async () =>
            {
                await LoadData(); 
            });

            Categories.InsertRange(0, tempCategories);

            SelectedCategory = Categories.FirstOrDefault();
            if (SelectedCategory != null)
            {
                SelectedCategoryPKey = SelectedCategory.CategoryPKey;
                tempQuestions = SelectedCategory.Questions;

                sortedQuestions = from item in tempQuestions
                                  orderby item.Sortorder
                                  group item by new { item.Location_id, item.Locationname, item.Sortorder } into questGroup
                                  select new QuestionGrouping(questGroup.Key.Locationname, questGroup);

                string lastLoc = "";
                int idx = 0;

                try
                {
                    foreach (QuestionGrouping q in sortedQuestions)
                    {
                        if (lastLoc != q.Key)
                        {                         
                            tempQuestionsGrouped.Add(q);
                            idx++;
                        }
                        else
                        {
                            tempQuestionsGrouped[idx - 1].InsertItems(q.GetItemList());
                        }
                        lastLoc = q.Key;
                    }

                    QuestionsGrouped.AddRange(tempQuestionsGrouped);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

            }
            else
            {
                SelectedCategoryPKey = "0";
                ForceQuestionReload = true;
            }

            IsLoaded = true;
            EnableCategories = true;
        }


        public async Task LoadData()
        {
            try
            {
                tempCategories.Clear();

                Category cat1 = new Category();
                cat1.CategoryPKey = "C1";
                cat1.Name = "Facts";
                ObservableCollection<Question> questList1 = new ObservableCollection<Question>();
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q1", Label = "Enter name of current month", Location_id = 1, Locationname = "Group 1", Sortorder = 1 });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q2", Label = "Enter name of current weekday", Location_id = 1, Locationname = "Group 1", Sortorder = 2 });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q3", Label = "Enter current year", Location_id = 1, Locationname = "Group 1", Sortorder = 3, IsNumeric = true });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q4", Label = "Enter current hour", Location_id = 1, Locationname = "Group 1", Sortorder = 4, IsNumeric = true });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q5", Label = "Enter current minute", Location_id = 1, Locationname = "Group 1", Sortorder = 5, IsNumeric = true });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q6", Label = "Enter day or night", Location_id = 2, Locationname = "Group 2", Sortorder = 6 });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q7", Label = "Enter name of city", Location_id = 2, Locationname = "Group 2", Sortorder = 7 });
                questList1.Add(new Question { CategoryPKey = "C1", Category = cat1, Controltype = "text", QuestionPKey = "C1Q8", Label = "Enter name of state", Location_id = 2, Locationname = "Group 2", Sortorder = 8 });

                Question SwitchQuestionC1Q9 = new Question { CategoryPKey = "C1", Category = cat1, Controltype = "switch", QuestionPKey = "C1Q9", Label = "Facts Switch 1 Yes or No", Location_id = 2, Locationname = "Group 2", Sortorder = 9 };
                ObservableCollection<QuestionOption> questOptListC1Q9 = new ObservableCollection<QuestionOption>();
                QuestionOption YesOptionC1Q9 = new QuestionOption { Question = SwitchQuestionC1Q9, QuestionPKey = SwitchQuestionC1Q9.QuestionPKey, QuestionOptionPKey = "C1Q9YES", Text = "Yes", Value = "Yes" };
                QuestionOption NoOptionC1Q9 = new QuestionOption { Question = SwitchQuestionC1Q9, QuestionPKey = SwitchQuestionC1Q9.QuestionPKey, QuestionOptionPKey = "C1Q9NO", Text = "No", Value = "No" };
                questOptListC1Q9.Add(YesOptionC1Q9);
                questOptListC1Q9.Add(NoOptionC1Q9);
                SwitchQuestionC1Q9.Options = questOptListC1Q9;
                questList1.Add(SwitchQuestionC1Q9);

                Question SwitchQuestionC1Q10 = new Question { CategoryPKey = "C1", Category = cat1, Controltype = "switch", QuestionPKey = "C1Q10", Label = "Facts Switch 2 Yes or No", Location_id = 2, Locationname = "Group 2", Sortorder = 10 };
                ObservableCollection<QuestionOption> questOptListC1Q10 = new ObservableCollection<QuestionOption>();
                QuestionOption YesOptionC1Q10 = new QuestionOption { Question = SwitchQuestionC1Q10, QuestionPKey = SwitchQuestionC1Q10.QuestionPKey, QuestionOptionPKey = "C1Q10YES", Text = "Yes", Value = "Yes" };
                QuestionOption NoOptionC1Q10 = new QuestionOption { Question = SwitchQuestionC1Q10, QuestionPKey = SwitchQuestionC1Q10.QuestionPKey, QuestionOptionPKey = "C1Q10NO", Text = "No", Value = "No" };
                questOptListC1Q10.Add(YesOptionC1Q10);
                questOptListC1Q10.Add(NoOptionC1Q10);
                SwitchQuestionC1Q10.Options = questOptListC1Q10;
                questList1.Add(SwitchQuestionC1Q10);

                cat1.Questions = questList1;
                tempCategories.Add(cat1);

                Category cat2 = new Category();
                cat2.CategoryPKey = "C2";
                cat2.Name = "Favorites";
                ObservableCollection<Question> questList2 = new ObservableCollection<Question>();
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q1", Label = "Enter favorite color", Location_id = 3, Locationname = "Group 3", Sortorder = 1 });
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q2", Label = "Enter favorite number", Location_id = 3, Locationname = "Group 3", Sortorder = 2, IsNumeric = true });
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q3", Label = "Enter favorite car model", Location_id = 3, Locationname = "Group 3", Sortorder = 3});
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q4", Label = "Enter favorite season", Location_id = 3, Locationname = "Group 3", Sortorder = 4 });
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q5", Label = "Enter favorite holiday", Location_id = 3, Locationname = "Group 3", Sortorder = 5});
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q6", Label = "Enter favorite pet's name", Location_id = 4, Locationname = "Group 4", Sortorder = 6 });
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q7", Label = "Enter favorite song", Location_id = 4, Locationname = "Group 4", Sortorder = 7 });
                questList2.Add(new Question { CategoryPKey = "C2", Category = cat2, Controltype = "text", QuestionPKey = "C2Q8", Label = "Enter favorite cookie", Location_id = 4, Locationname = "Group 4", Sortorder = 8 });

                Question SwitchQuestionC2Q9 = new Question { CategoryPKey = "C2", Category = cat2, Controltype = "switch", QuestionPKey = "C2Q9", Label = "Favorites Switch 1 Yes or No", Location_id = 4, Locationname = "Group 4", Sortorder = 9 };
                ObservableCollection<QuestionOption> questOptListC2Q9 = new ObservableCollection<QuestionOption>();
                QuestionOption YesOptionC2Q9 = new QuestionOption { Question = SwitchQuestionC2Q9, QuestionPKey = SwitchQuestionC2Q9.QuestionPKey, QuestionOptionPKey = "C2Q9YES", Text = "Yes", Value = "Yes" };
                QuestionOption NoOptionC2Q9 = new QuestionOption { Question = SwitchQuestionC2Q9, QuestionPKey = SwitchQuestionC2Q9.QuestionPKey, QuestionOptionPKey = "C2Q9NO", Text = "No", Value = "No" };
                questOptListC2Q9.Add(YesOptionC2Q9);
                questOptListC2Q9.Add(NoOptionC2Q9);
                SwitchQuestionC2Q9.Options = questOptListC2Q9;
                questList2.Add(SwitchQuestionC2Q9);

                Question SwitchQuestionC2Q10 = new Question { CategoryPKey = "C2", Category = cat2, Controltype = "switch", QuestionPKey = "C2Q10", Label = "Favorites Switch 2 Yes or No", Location_id = 4, Locationname = "Group 4", Sortorder = 10 };
                ObservableCollection<QuestionOption> questOptListC2Q10 = new ObservableCollection<QuestionOption>();
                QuestionOption YesOptionC2Q10 = new QuestionOption { Question = SwitchQuestionC2Q10, QuestionPKey = SwitchQuestionC2Q10.QuestionPKey, QuestionOptionPKey = "C2Q10YES", Text = "Yes", Value = "Yes" };
                QuestionOption NoOptionC2Q10 = new QuestionOption { Question = SwitchQuestionC2Q10, QuestionPKey = SwitchQuestionC2Q10.QuestionPKey, QuestionOptionPKey = "C2Q10NO", Text = "No", Value = "No" };
                questOptListC2Q10.Add(YesOptionC2Q10);
                questOptListC2Q10.Add(NoOptionC2Q10);
                SwitchQuestionC2Q10.Options = questOptListC2Q10;
                questList2.Add(SwitchQuestionC2Q10);



                cat2.Questions = questList2;
                tempCategories.Add(cat2);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }


        public async Task OnItemTapped(object item)
        {
            try
            {
          
                EnableCategories = false;

                if (SelectedCategoryPKey == null || item == null)
                {
                    EnableCategories = true;
                    return;
                }

                var catItem = (Category)item;

                if (catItem.CategoryPKey != SelectedCategoryPKey || ForceQuestionReload)
                {
                    if (ForceQuestionReload)
                    {
                        ForceQuestionReload = false;
                    }

                    // Try up to 10 times in case method is currently busy
                    for (var x = 0; x < 10; x++)
                    {
                        var result = await LoadQuestions(catItem);
                        if (result == "busy")
                        {
                            Debug.WriteLine("-----> in OnItemTapped() BUSY, barcode = " + catItem.Name);
                            await Task.Delay(500);
                        }
                        else
                        {
                            break;
                        }
                    }
                }


                EnableCategories = true;
            }
            catch (Exception e)
            {
                EnableCategories = true;
                Debug.WriteLine(e.Message);
            }
        }


        public async Task<QuestionUpdateResult> OnQuestionChanged(Question theQuest, object newVal)
        {
            QuestionUpdateResult result = new QuestionUpdateResult();
            try
            {
                return await Task.Run(async () =>
                {
                    IsBusy = true;
                    var catData = theQuest.Category;

                    if (newVal == null)
                    {
                        newVal = string.Empty;
                    }

                    string strNewVal = newVal.ToString();
                    theQuest.Value = strNewVal;


                    // Instead of saving the value to the database and retrieving everything, just update the ObservableCollections 
                    var catpair = Categories.Select((Value, Index) => new { Value, Index }).Single(p => p.Value.CategoryPKey == theQuest.Category.CategoryPKey);
                    if (catpair != null && catpair.Index != -1)
                    {
                        if (QuestionsGrouped.Count > 0)
                        {
                            int theIndex = -1;
                            QuestionGrouping theValue = null;
                            var groupPair = new { Value = theValue, Index = theIndex };

                            var groupPairList = QuestionsGrouped.Select((Value, Index) => new { Value, Index }).Where(z => z.Value.Key == theQuest.Locationname);
                            if (groupPairList != null && groupPairList.Count() > 1)
                            {
                                foreach (var x in groupPairList)
                                {
                                    if (x.Value.GetItemList().Any(qu => qu.QuestionPKey == theQuest.QuestionPKey))
                                    {
                                        theIndex = x.Index;
                                        theValue = x.Value;
                                        groupPair = new { Value = theValue, Index = theIndex };
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                groupPair = QuestionsGrouped.Select((Value, Index) => new { Value, Index }).SingleOrDefault(z => z.Value.Key == theQuest.Locationname);
                            }

                            if (groupPair != null && groupPair.Index != -1)
                            {
                                var qpair = groupPair.Value.Select((Value, Index) => new { Value, Index }).Single(q => q.Value.QuestionPKey == theQuest.QuestionPKey);
                                if (qpair.Index != -1)
                                {
                                    // update question
                                    Question source = theQuest;
                                    Question target = QuestionsGrouped[groupPair.Index][qpair.Index];
                                    var sourceType = typeof(Question);
                                    var targetType = typeof(Question);

                                    foreach (var sourceProperty in sourceType.GetProperties())
                                    {
                                        if (sourceProperty.Name == "Value")
                                        {
                                            var targetProperty = targetType.GetProperty(sourceProperty.Name);
                                            if (targetProperty != null)
                                            {
                                                if (targetProperty.GetSetMethod() != null)
                                                {
                                                    MainThread.BeginInvokeOnMainThread(() =>
                                                    {
                                                        targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                                                    });
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        // update category
                        Category catSource = catData;
                        Category catTarget = Categories[catpair.Index];
                        var CatSourceType = typeof(Category);
                        var CatTargetType = typeof(Category);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            foreach (var sourceProperty in CatSourceType.GetProperties())
                            {
                                if (sourceProperty.Name == "TotalAnsweredQuestions" || sourceProperty.Name == "TotalEligibleQuestions")
                                {
                                    var targetProperty = CatTargetType.GetProperty(sourceProperty.Name);
                                    if (targetProperty != null)
                                    {
                                        if (targetProperty.GetSetMethod() != null)
                                        {
                                            targetProperty.SetValue(catTarget, sourceProperty.GetValue(catSource, null), null);
                                        }
                                    }
                                }
                            }
                        });
                    }

                    result.QuestionObj = theQuest;
                    result.ReturnVal = "1";
                    IsBusy = false;
                    
                    //if (theQuest.QuestionPKey == "C2Q9") // mimic analyze desc
                    //{
                    //    await RefreshData();
                    //}

                    return result;
                });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return result;
            }
        }



        public async Task<string> LoadQuestions(Category catItem)
        {
            try
            {
                if (LoadingQuestions)
                {
                    return "busy";
                }

                LoadingQuestions = true;
                tempQuestionsGrouped.Clear();
                QuestionsGrouped.Clear();

                await Task.Delay(50);

                SelectedCategoryPKey = catItem.CategoryPKey;

                sortedQuestions = from qitem in catItem.Questions
                                  where qitem.Visible
                                  orderby qitem.Sortorder
                                  group qitem by new { qitem.Location_id, qitem.Locationname, qitem.Sortorder } into questGroup
                                  select new QuestionGrouping(questGroup.Key.Locationname, questGroup);

                string lastLoc = "";
                int idx = 0;
                foreach (QuestionGrouping q in sortedQuestions)
                {
                    if (lastLoc != q.Key)
                    {
                        tempQuestionsGrouped.Add(q);
                        idx++;
                    }
                    else
                    {
                        tempQuestionsGrouped[idx - 1].InsertItems(q.GetItemList());
                    }
                    lastLoc = q.Key;
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (QuestionGrouping questgrp in tempQuestionsGrouped)
                    {
                        QuestionsGrouped.Add(questgrp);
                    }
                });


                LoadingQuestions = false;
                return "ok";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "ok";
            }
        }


        public async Task RefreshData(bool isReset = false)
        {

            try
            {

                // remember current SelectedCategory
                var theLastSelectedCategoryPKey = "";
                if (SelectedCategory == null)
                {
                    if (LastSelectedCategoryPKey != "")
                    {
                        theLastSelectedCategoryPKey = LastSelectedCategoryPKey;
                        LastSelectedCategoryPKey = "";
                    }
                }
                else
                {
                    theLastSelectedCategoryPKey = SelectedCategory.CategoryPKey;
                }

                if (isReset)
                {
                    await LoadData();
                } 
                else
                {
                    tempCategories = Categories.ToList();
                }

                EnableCategories = true;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Categories.Clear();
                    foreach (Category b in tempCategories)
                    {
                        Categories.Add(b);
                    }

                    if (Categories.Count > 0)
                    {
                        if (string.IsNullOrEmpty(theLastSelectedCategoryPKey))
                        {
                            SelectedCategory = Categories.First();
                            if (SelectedCategory == null)
                            {
                                SelectedCategoryPKey = "0";
                            }
                        }
                        else
                        {
                            Category theSelectedCat = null;
                            theSelectedCat = Categories.Select(s => s).Where(x => x.CategoryPKey == theLastSelectedCategoryPKey).FirstOrDefault();
                            if (theSelectedCat == null)
                            {
                                SelectedCategory = Categories.FirstOrDefault();
                                if (SelectedCategory == null)
                                {
                                    SelectedCategoryPKey = "0";
                                }
                            }
                            else
                            {
                                SelectedCategory = theSelectedCat;
                            }
                        }
                    }
                    else
                    {
                        QuestionsGrouped.Clear();
                    }

                });

            }
            catch (Exception e)
            {
                EnableCategories = true;
                Debug.WriteLine(e);
            }
        }


        public async Task OnClicked(object buttonText)
        {
            switch ((string)buttonText)
            {
                case "Reset":
                    ForceQuestionReload = true;
                    await RefreshData(true);                                    
                    break;

                default:
                    break;
            }
        }




    }
}
