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
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }

        private void GoSFO_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("http://www.sfosquad.com"));
        }

        private void dinosoftlabs_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.flaticon.com/authors/dinosoftlabs"));
        }
    }
}