using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class MyAppContext
    {
        private static List<User> users = new List<User>();
        private static List<Note> notes = new List<Note>();

        private static int lastNoteId = 0;
        public static int NewNoteId => lastNoteId++;
        private static int lastUserId = 0;
        public static int NewUserId => lastUserId++;

        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public MyAppContext()
        {
        }

        public List<User> Users { get => users; set => users = value; }
        public List<Note> Notes { get => notes; set => notes = value; }
    }
}
