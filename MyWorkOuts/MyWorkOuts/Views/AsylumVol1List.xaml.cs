using MyWorkOuts.Data;
using MyWorkOuts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyWorkOuts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsylumVol1List : ContentPage
    {
        public AsylumVol1List()
        {
            InitializeComponent();
            List<string> workOutList = BeachBodyWorkOutList.AsylumVol1();

            MyAsylumVol1WorkOutList.ItemsSource = BeachBodyWorkOutList.AsylumVol1();
        }
    }
}