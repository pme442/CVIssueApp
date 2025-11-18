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
    public class TestPageViewModel : ObservableObject
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

        private TimeSpan? _selectedTime;
        public TimeSpan? SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        private TimeSpan? _selectedNullTime;
        public TimeSpan? SelectedNullTime
        {
            get { return _selectedNullTime; }
            set
            {
                _selectedNullTime = value;
                OnPropertyChanged(nameof(SelectedNullTime));
            }
        }

        public string SelectedCategoryPKey { get; set; }

        private ICommand _clickCommandButton;
        public ICommand ClickCommandButton
        {
            get { return _clickCommandButton ?? (_clickCommandButton = new Command(async (x) => await OnClicked(x))); }
        }

        private ICommand itemViewTappedCommand;
        public ICommand ItemTappedCommand
        {
            get { return itemViewTappedCommand ?? (itemViewTappedCommand = new Command((x) => OnItemTapped(x))); }
        }

        List<Category> tempCategories = new List<Category>();
        private string LastSelectedCategoryPKey = "";

        public TestPageViewModel()
        {
            Categories = new ObservableCollectionFast<Category>();
            SelectedTime = new TimeSpan(11,25,00);
            SelectedNullTime = null;
        }

        public async void OnAppearing()
        {

            await Task.Run(async () =>
            {
                await LoadData();
            });

            Categories.InsertRange(0, tempCategories);

            //SelectedCategory = Categories.FirstOrDefault();
            //if (SelectedCategory != null)
            //{
            //    SelectedCategoryPKey = SelectedCategory.CategoryPKey;


            //}
            //else
            //{
                SelectedCategoryPKey = "0";
            //}
            IsLoaded = true;

        }



        public async Task LoadData()
        {
            try
            {
                tempCategories.Clear();

                Category cat1 = new Category();
                cat1.CategoryPKey = "C1";
                cat1.Name = "Facts";
                
                tempCategories.Add(cat1);

                Category cat2 = new Category();
                cat2.CategoryPKey = "C2";
                cat2.Name = "Favorites";
                
                tempCategories.Add(cat2);

                Category cat3 = new Category();
                cat3.CategoryPKey = "C3";
                cat3.Name = "Holidays";

                tempCategories.Add(cat3);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                    tempCategories.Clear();
                    tempCategories = Categories.ToList();
                }


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
                                SelectedCategory = null;
                                SelectedCategory = theSelectedCat;
                            }
                        }
                    }                    

                });

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private async void OnItemTapped(object item)
        {
            try
            {
                if (item is Category cat)
                {
                    SelectedCategory = cat;
                    await Application.Current.MainPage.DisplayAlert("Alert", "You selected " + cat.Name, "OK");
                }              
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        public async Task OnClicked(object buttonText)
        {
            switch ((string)buttonText)
            {
                case "Reset":
                    await RefreshData(true);
                    break;

                default:
                    break;
            }
        }

    }
}
