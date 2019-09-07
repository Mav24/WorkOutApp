using System;
using MarcTron.Plugin;
using SQLite;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var workOutsList = await _connection.Table<WorkOutModel>().ToListAsync();
            _workOuts = new ObservableCollection<WorkOutModel>(workOutsList.OrderBy(x => x.Date).ToList());
            var workOutCount = _workOuts.Count;
            var completed = _workOuts.Count(x => x.Done);
            var workOutDaysLeft = workOutCount - completed;
            MyWorkOutList.ItemsSource = _workOuts;

            if (workOutCount > 0)
            {
                WorkOutTitle.Text = _workOuts[0].Title.ToString();
                WorkoutDays.Text = $"\n{workOutDaysLeft} Days left";
            }
            else
            {
                WorkOutTitle.Text = "Click the create workout button to start a workout program!";
                WorkoutDays.Text = "";
            }
            
            // Needs work need to get current days work out and count of how many days left
            //var daysLeft = _workOuts.Count;
            //Status.Text = $"Current WorkOut Days Left: {daysLeft.ToString()}";
            
        }
        async void Create_WorkOut_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Warning!!", "Are you sure you want to create a new workout calendar? Current calendar will be deleted.", "Yes", "No"))
            {

                await _connection.DropTableAsync<WorkOutModel>();
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