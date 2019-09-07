
using MyWorkOuts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MyWorkOuts
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Not sure if i need this. I think I added this when i was having issue with the main page updating
            //Routing.RegisterRoute("CreateWorkOutCalendar", typeof(CreateWorkOutCalendar));
        }
    }
}
