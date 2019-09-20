using MyWorkOuts.Models;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkOutPrograms : ContentPage
    {
        public WorkOutPrograms()
        {
            InitializeComponent();
            MyProgramsList.ItemsSource = new List<ProgramList>
            {
                new ProgramList { ProgramTitle = "Insanity", DaysToWorkOut = "63 Day's", ImageUrl = "insanity1.png" },
                new ProgramList { ProgramTitle = "Asylum Vol 1", DaysToWorkOut = "30 Day's", ImageUrl = "asylumvol1.png" },
                new ProgramList { ProgramTitle = "Asylum Vol 2", DaysToWorkOut = "28 Day's", ImageUrl = "asylumvol2.png" },
                new ProgramList { ProgramTitle = "Asylum 1 + 2", DaysToWorkOut = "55 Day's", ImageUrl = "asylumoneandtwo.png" },
                new ProgramList { ProgramTitle = "Ten Minute Trainer", DaysToWorkOut = "28 Day's", ImageUrl = "tenminutetrainer1.png" },
                new ProgramList { ProgramTitle = "Max 30", DaysToWorkOut = "56 Day's", ImageUrl = "max30.png" }
            };
        }
        private void WorkoutProgramsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MyProgramsList.SelectedItem = null;
        }
    }
}