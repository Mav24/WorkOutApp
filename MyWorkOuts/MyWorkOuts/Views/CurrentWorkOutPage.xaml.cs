using System;
using MarcTron.Plugin;
using SQLite;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWorkOutPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<WorkOutModel> _workOuts;

        public CurrentWorkOutPage()
        {
            InitializeComponent();
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _connection.CreateTableAsync<WorkOutModel>();
            _workOuts = new ObservableCollection<WorkOutModel>((await _connection.Table<WorkOutModel>().ToListAsync()).OrderBy(x => x.Date).ToList());
            var workOutDaysLeft  = _workOuts.Count - _workOuts.Count(x => x.Done);
            MyWorkOutList.ItemsSource = _workOuts;           
            
            if (_workOuts.Count <= 0)
            {
                WorkOutTitle.Text = "Click the CREATE WORKOUT button to start a workout program!";
                WorkoutDays.Text = "";
            }
            else
            {
                if (workOutDaysLeft <= 0)
                {
                    WorkOutTitle.Text = _workOuts[0].Title.ToString();
                    WorkoutDays.Text = "\nCompleted!";
                }
                else
                {
                    // This needs work.. This scrolls but maybe need to get date
                    WorkOutTitle.Text = _workOuts[0].Title.ToString();
                    WorkoutDays.Text = $"\n{workOutDaysLeft} Days left";
                    //ScrollToCurrentWorkOut(_workOuts.Count);

                }

            }
        }

        private void ScrollToCurrentWorkOut(int workOutCount)
        {
            for (int i = 0; i < workOutCount; i++)
            {
                if (_workOuts[i].Date == DateTime.Now.Date)
                {
                    MyWorkOutList.ScrollTo(_workOuts[i], ScrollToPosition.Start, true);
                }
            }
        }

        async void Create_WorkOut_Clicked(object sender, EventArgs e)
        {
            var workOutCount = _workOuts.Count;
            if (workOutCount > 0)
            {
                if (await DisplayAlert("Warning!!", $"Are you sure you want to create a new workout calendar? Current calendar {WorkOutTitle.Text} will be deleted.", "Yes", "No"))
                {
                    await _connection.DropTableAsync<WorkOutModel>();
                    await Shell.Current.Navigation.PushModalAsync(new CreateWorkOutCalendar());
                }
            }
            else
            {
                await Shell.Current.Navigation.PushModalAsync(new CreateWorkOutCalendar());
            }
        }

        async void MyWorkOutList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var workOutTapped = e.Item as WorkOutModel;

            if (await DisplayAlert("Completed!", $"{workOutTapped.WorkOutTitle}", "Yes", "No"))
            {
                workOutTapped.Done = true;
                await _connection.UpdateAsync(workOutTapped);
                MyWorkOutList.SelectedItem = null;
                //var workOutsList = await _connection.Table<WorkOutModel>().ToListAsync();
                //_workOuts = new ObservableCollection<WorkOutModel>(workOutsList.OrderBy(x => x.Date).ToList());
                
                OnAppearing();
            }
            else
            {
                workOutTapped.Done = false;
                await _connection.UpdateAsync(workOutTapped);
                MyWorkOutList.SelectedItem = null;
                OnAppearing();
            }
        }
    }
}