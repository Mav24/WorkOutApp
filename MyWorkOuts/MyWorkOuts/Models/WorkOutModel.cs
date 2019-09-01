using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace MyWorkOuts.Models
{
    public class WorkOutModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string WorkOutTitle { get; set; }
        public DateTime Date { get; set; }
        public bool Done { get; set; }
    }
}
