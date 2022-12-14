using System;
using SQLite;

namespace HappinessIndex.Models
{
    public class PersonalNote
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int UserID { get; set; }

        public DateTime Date { get; set; }

        public string Note { get; set; }
    }
}