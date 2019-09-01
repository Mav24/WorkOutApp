
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
            Routing.RegisterRoute("CreateWorkOutCalendar", typeof(CreateWorkOutCalendar));
        }
    }
}
