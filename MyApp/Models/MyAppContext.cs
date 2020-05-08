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
        private static int lastUserId = 0;

        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public MyAppContext()
        {
        }

        public Note GetNoteById(int noteId)
        {
            return notes.FirstOrDefault(a => a.NoteId == noteId);
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(u => u.UserId == userId);
        }

        public IEnumerable<Note> GetNotesByUserId(int userId)
        {
            return notes.Where(a => a.User.UserId == userId).ToList();
        }

        public void Save(Note note)
        {
            note.NoteId = ++lastNoteId;
            notes.Add(note);
        }

        public void Save(User user)
        {
            user.UserId = ++lastUserId;
            users.Add(user);
        }

        public bool IsEmailAlreadyRegistered(string email)
        {
            return users.Where(u => u.Email == email).Count() > 0;
        }

        public User Login(string email, string password)
        {
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        internal void DeleteNote(int id)
        {
            Note note = notes.First(a => a.NoteId == id);
            notes.Remove(note);
        }
    }
}
