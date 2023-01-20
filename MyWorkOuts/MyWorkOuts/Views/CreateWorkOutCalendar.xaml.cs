using System;
using MarcTron.Plugin;
using SQLite;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyWorkOuts.Data;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateWorkOutCalendar : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<WorkOutModel> _workOuts;

        List<string> workOutList;
        string selectedProgram;
        public CreateWorkOutCalendar()
        {
            InitializeComponent();
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
        }
        public CreateWorkOutCalendar(string programTitle)
        {
            InitializeComponent();
            
            selectedProgram = programTitle;
            Program.SelectedItem = programTitle;
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
            _connection.DropTableAsync<WorkOutModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _connection.CreateTableAsync<WorkOutModel>();
            _workOuts = new ObservableCollection<WorkOutModel>((await _connection.Table<WorkOutModel>().ToListAsync()).OrderBy(x => x.Date).ToList());
            
        }

        private void Program_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProgram = Program.SelectedItem.ToString();
            switch (selectedProgram)
            {
                case "Asylum Vol 1":
                    workOutList = BeachBodyWorkOutList.AsylumVol1();
                    break;
                case "Asylum Vol 2":
                    workOutList = BeachBodyWorkOutList.AsylumVol2();
                    break;
                case "Asylum 1 + 2":
                    workOutList = BeachBodyWorkOutList.AsylumVol1and2();
                    break;
                case "Insanity":
                    workOutList = BeachBodyWorkOutList.Insanity();
                    break;
                case "Max 30":
                    workOutList = BeachBodyWorkOutList.Max30();
                    break;
                case "10 Minute Trainer":
                    workOutList = BeachBodyWorkOutList.TenMinTrainer();
                    break;
                case "T25 Alpha and Beta":
                    workOutList = BeachBodyWorkOutList.T25AlphaBeta();
                    break;
                case "T25 Gamma":
                    workOutList = BeachBodyWorkOutList.T25Gamma();
                    break;
                case "TRANSFORM: 20":
                    workOutList = BeachBodyWorkOutList.TransForm20();
                    break;
                case "Lift4":
                    workOutList = BeachBodyWorkOutList.Lift4();
                    break;
                case "Shift Shop":
                    workOutList = BeachBodyWorkOutList.ShiftShop();
                    break;
                default:
                    break;
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {

            await CreateNewCalendar();
            //await Shell.Current.Navigation.PopModalAsync();
            await Shell.Current.GoToAsync("//CurrentWorkOut");
        }

        private async Task CreateNewCalendar()
        {
            var datePicked = startDate.Date;
            try
            {
                for (int i = 0; i < workOutList.Count; i++)
                {
                    WorkOutModel itemsList = new WorkOutModel()
                    {
                        Title = selectedProgram,
                        WorkOutTitle = workOutList[i],
                        Date = datePicked.AddDays(i),
                        Done = false

                    };
                    await _connection.InsertAsync(itemsList);
                    _workOuts.Add(itemsList);
                }

                await DisplayAlert("Calendar Created", $"{selectedProgram} WorkOut Calendar has been created.", "Great!");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Something went wrong! please try again", "OK");
            }
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}