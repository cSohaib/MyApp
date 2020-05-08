using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class NotesController : Controller
    {
        private MyAppContext db = new MyAppContext();

        public ActionResult Index()
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            var currentUserNotes = db.Notes.Where(a => a.User.UserId == (int)Session["UserId"]).ToList();
            return View(currentUserNotes);
        }

        public ActionResult Details(int id)
        {
            Note note = db.Notes.FirstOrDefault(a => a.NoteId == id);

            if (note is null)
                return HttpNotFound();

            var model = new NoteViewModel(note);
            return View(model);
        }

        public ActionResult Create()
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            var model = new NoteViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteViewModel model)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            if (ModelState.IsValid)
            {
                Note note = new Note()
                {
                    NoteId = MyAppContext.NewNoteId,
                    Content = model.Content,
                    Title = model.Title,
                    User = db.Users.First(u => u.UserId == (int)Session["UserId"])
                };
                db.Notes.Add(note);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            Note note = db.Notes.FirstOrDefault(a => a.NoteId == id);

            if (note is null || note.User.UserId != (int)Session["UserId"])
                return HttpNotFound();

            var model = new NoteViewModel(note);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoteViewModel model)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            if (ModelState.IsValid)
            {
                Note note = db.Notes.FirstOrDefault(a => a.NoteId == model.Id);
                note.Content = model.Content;
                note.Title = model.Title;

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            Note note = db.Notes.FirstOrDefault(a => a.NoteId == id);

            if (note is null || note.User.UserId != (int)Session["UserId"])
                return HttpNotFound();

            var model = new NoteViewModel(note);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            Note note = db.Notes.First(a => a.NoteId == id);
            db.Notes.Remove(note);
            return RedirectToAction("Index");
        }
    }
}
