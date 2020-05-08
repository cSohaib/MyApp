using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class UsersController : Controller
    {
        private MyAppContext db = new MyAppContext();

        public ActionResult Account()
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            var user = db.Users.First(u => u.UserId == (int)Session["UserId"]);
            return View(user);
        }

        public ActionResult Login()
        {
            if (Session["UserId"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User user;
            try
            {
                user = db.Users.First(u => u.Email == model.Email && u.Password == model.Password);
            }
            catch
            {
                ModelState.AddModelError("", "Wrong username or password");
                return View(model);
            }
            Session["UserId"] = user.UserId;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit()
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Index", "Home");

            var user = db.Users.First(u => u.UserId == (int)Session["UserId"]);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (Session["UserId"] is null)
                return RedirectToAction("Login", "Users");

            if (!ModelState.IsValid)
                return View(user);

            var usr = db.Users.First(u => u.UserId == (int)Session["UserId"]);
            usr.Firstname = user.Firstname;
            usr.Lastname = user.Lastname;
            return RedirectToAction("Account");
        }

        public ActionResult Register()
        {
            if (Session["UserId"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (db.Users.Where(u => u.Email == model.Email).Count() > 0)
            {
                ModelState.AddModelError("", "Email already registered");
                return View(model);
            }

            User user = new User
            {
                UserId = MyAppContext.NewUserId,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Password = model.Password
            };
            db.Users.Add(user);
            Session["UserId"] = user.UserId;
            return RedirectToAction("Account", "Users");


        }

        public ActionResult ChangePassword()
        {
            if (Session["UserId"] is null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = db.Users.First(u => u.UserId == (int)Session["UserId"]);
            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("", "Wrong password");
                return View(model);
            }
            user.Password = model.NewPassword;
            return RedirectToAction("Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserId"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
