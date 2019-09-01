using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcTron.Plugin;
using SQLite;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordMeasurementsPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Measurements> _measurements;
        public RecordMeasurementsPage()
        {
            InitializeComponent();
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
        }

        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Measurements>();
            var measurements = await _connection.Table<Measurements>().ToListAsync();
            _measurements = new ObservableCollection<Measurements>(measurements);
            base.OnAppearing();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ChestEntry.Text) ||
                    string.IsNullOrWhiteSpace(LeftArm.Text) ||
                    string.IsNullOrWhiteSpace(RightArm.Text) ||
                    string.IsNullOrWhiteSpace(Waist.Text) ||
                    string.IsNullOrWhiteSpace(Hip.Text) ||
                    string.IsNullOrWhiteSpace(LeftThigh.Text) ||
                    string.IsNullOrWhiteSpace(RightThigh.Text) ||
                    string.IsNullOrWhiteSpace(CurrentWeigh.Text))
                {
                    await DisplayAlert("Error", "All fields need to be filled out", "Ok");
                }
                else
                {
                    Measurements newMeasurements = new Measurements()
                    {
                        CurrentDate = DateTime.Now,
                        Chest = ChestEntry.Text,
                        LeftArm = LeftArm.Text,
                        RightArm = RightArm.Text,
                        Waist = Waist.Text,
                        Hip = Hip.Text,
                        LeftThigh = LeftThigh.Text,
                        RightThigh = RightThigh.Text,
                        CurrentWeigh = CurrentWeigh.Text
                    };

                    // clear entries
                    ChestEntry.Text = string.Empty;
                    LeftArm.Text = string.Empty;
                    RightArm.Text = string.Empty;
                    Waist.Text = string.Empty;
                    Hip.Text = string.Empty;
                    LeftThigh.Text = string.Empty;
                    RightThigh.Text = string.Empty;
                    CurrentWeigh.Text = string.Empty;
                    await _connection.InsertAsync(newMeasurements);
                    _measurements.Add(newMeasurements);

                    await DisplayAlert("Measurements Add", "Your measurements were saved", "Great");
                }

            }
            catch (Exception)
            {
                await DisplayAlert("Error!", "Something went wrong!", "Ok");
            }
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {

        }
    }
}