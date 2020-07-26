using System;
using MarcTron.Plugin;
using SQLite;
using System.Collections.ObjectModel;
using MyWorkOuts.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PastRecordPage : ContentPage
    {

        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Measurements> _measurements;
        public PastRecordPage()
        {
            InitializeComponent();
            _connection = MtSql.Current.GetConnectionAsync("myworkouts.db3");
        }
        protected override async void OnAppearing()
        {
            await _connection.CreateTableAsync<Measurements>();
            _measurements = new ObservableCollection<Measurements>((await _connection.Table<Measurements>().
                ToListAsync()).OrderBy(x => x.CurrentDate).ToList());

            MeasurementsList.ItemsSource = _measurements;
            base.OnAppearing();
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            var delRecord = (sender as MenuItem).CommandParameter as Measurements;
            if (await DisplayAlert("Warning!", $"Are you sure you want to delete {delRecord.CurrentDate} Record", "Yes", "No"))
            {
                await _connection.DeleteAsync(delRecord);
                _measurements.Remove(delRecord);
            }
        }

        private void MeasurementsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MeasurementsList.SelectedItem = null;

        }
    }
}