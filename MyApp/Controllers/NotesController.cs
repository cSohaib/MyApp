using MyApp.Models;
using System.Web.Mvc;

namespace MyApp.Controllers
{
    public class NotesController : Controller
    {
        private MyAppContext db = new MyAppContext();

        public ActionResult Index()
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            int currentUserId = (int)Session["UserId"];
            var notes = db.GetNotesByUserId(currentUserId);
            return View(notes);
        }

        public ActionResult Details(int id)
        {
            Note note = db.GetNoteById(id);

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
                    Content = model.Content,
                    Title = model.Title,
                    User = db.GetUserById((int)Session["UserId"])
                };

                db.Save(note);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            Note note = db.GetNoteById(id);

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
                Note note = db.GetNoteById(model.Id);
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

            Note note = db.GetNoteById(id);

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

            db.DeleteNote(id);
            return RedirectToAction("Index");
        }
    }
}
