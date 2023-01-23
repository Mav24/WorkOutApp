using MarcTron.Plugin;
using MyWorkOuts.Models;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkOutPrograms : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<WorkOutModel> _workOuts;
        public WorkOutPrograms()
        {
            InitializeComponent();
            MyProgramsList.ItemsSource = WorkoutPrograms();
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var workOutsList = await _connection.Table<WorkOutModel>().ToListAsync();
            _workOuts = new ObservableCollection<WorkOutModel>(workOutsList);
            var workOutCount = _workOuts.Count;
            var completed = _workOuts.Count(x => x.Done);
            var workOutDaysLeft = workOutCount - completed;
            if (workOutCount > 0)
            {
                CurrentProgram.Text = $"Current Program {_workOuts[0].Title.ToString()}";
                WorkoutDays.Text = $"\n{workOutDaysLeft} Days Left";
            }
            else
            {
                CurrentProgram.Text = "Select a Program!";
                WorkoutDays.Text = "";
            }


        }

        private static List<ProgramList> WorkoutPrograms()
        {
            return new List<ProgramList>
            {
                new ProgramList { ProgramTitle = "Insanity", DaysToWorkOut = "63 Day's", ImageUrl = "insanity1.png" },
                new ProgramList { ProgramTitle = "Asylum Vol 1", DaysToWorkOut = "30 Day's", ImageUrl = "asylumvol1.png" },
                new ProgramList { ProgramTitle = "Asylum Vol 2", DaysToWorkOut = "28 Day's", ImageUrl = "asylumvol2.png" },
                new ProgramList { ProgramTitle = "Asylum 1 + 2", DaysToWorkOut = "55 Day's", ImageUrl = "asylumoneandtwo.png" },
                new ProgramList { ProgramTitle = "T25 Alpha and Beta", DaysToWorkOut = "70 Day's", ImageUrl = "t25alphabeta.png"},
                new ProgramList { ProgramTitle = "T25 Gamma", DaysToWorkOut = "28 Day's", ImageUrl = "t25gamma.png"},
                new ProgramList { ProgramTitle = "10 Minute Trainer", DaysToWorkOut = "28 Day's", ImageUrl = "tenminutetrainer1.png" },
                new ProgramList { ProgramTitle = "Max 30", DaysToWorkOut = "56 Day's", ImageUrl = "max30.png" },
                new ProgramList { ProgramTitle = "TRANSFORM: 20", DaysToWorkOut = "43 Days", ImageUrl = "transform20.png"},
                new ProgramList { ProgramTitle = "Lift4", DaysToWorkOut = "56 Days", ImageUrl = "lift4.png"},
                new ProgramList { ProgramTitle = "Shift Shop", DaysToWorkOut = "21 Days", ImageUrl = "shiftShop.png"}
            };
        }

        async void WorkoutProgramsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var workOutCount = _workOuts.Count;
            var selectedWorkOut = e.Item as ProgramList;
            var programTitle = selectedWorkOut.ProgramTitle;
            if (workOutCount > 0)
            {
                var currentWorkout = _workOuts[0].Title.ToString();
                if (await DisplayAlert("Warning!!", $"Are you sure you want to create a new workout calendar for {programTitle}?\nCurrent calendar {currentWorkout} will be deleted.", "Yes", "No"))
                {
                    await Shell.Current.Navigation.PushModalAsync(new CreateWorkOutCalendar(programTitle));
                }
            }
            else
            {
                await Shell.Current.Navigation.PushModalAsync(new CreateWorkOutCalendar(programTitle));
            }
            
            MyProgramsList.SelectedItem = null;
        }
    }
}