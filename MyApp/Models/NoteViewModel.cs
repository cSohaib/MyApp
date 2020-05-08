using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class NoteViewModel
    {
        public NoteViewModel()
        {

        }

        public NoteViewModel(Note note)
        {
            Id = note.NoteId;
            Title = note.Title;
            Content = note.Content;
        }

        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}