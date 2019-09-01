using System;
using MarcTron.Plugin;
using SQLite;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _connection.CreateTableAsync<WorkOutModel>();
            var workOutsList = await _connection.Table<WorkOutModel>().ToListAsync();
            _workOuts = new ObservableCollection<WorkOutModel>(workOutsList.OrderBy(x => x.Date).ToList());
            //startDate.Date = DateTime.Now;
            //Program.SelectedItem = string.Empty;
            
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
                default:
                    break;
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {

            await CreateNewCalendar();
            await Shell.Current.Navigation.PopModalAsync();
            //if (await DisplayAlert("Warning!!", "Are you sure you want to create a new workout calendar? Current calendar will be deleted.", "Yes", "No"))
            //{
            //    await _connection.DropTableAsync<WorkOutModel>();
            //    await CreateNewCalendar();
            //}

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
                //await Shell.Current.GoToAsync("//CurrentWorkOut");
                
                

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