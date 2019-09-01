using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWorkOuts.Models
{
    public class Measurements
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime CurrentDate { get; set; }
        public string Chest { get; set; }
        public string LeftArm { get; set; }
        public string RightArm { get; set; }
        public string Waist { get; set; }
        public string Hip { get; set; }
        public string LeftThigh { get; set; }
        public string RightThigh { get; set; }
        public string CurrentWeigh { get; set; }
    }
}
