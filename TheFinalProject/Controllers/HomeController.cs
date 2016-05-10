using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Humanizer;
using Microsoft.AspNet.Identity;
using TheFinalProject.Models;

namespace TheFinalProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private Models.DbContext db = new Models.DbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            return Profile(userId);

        }

        public ActionResult Profile(string id)
        {
            var userInfo = db.Users.Find(id);

            var userProfile = new ProfileVM()
            {
                Email = userInfo.Email,
                Photo = userInfo.Photo,
                Phone = userInfo.Phone,
                City = userInfo.City,
                State = userInfo.State,
                Zip = userInfo.Zip,
                MyTools = userInfo.MyTools.Select(t => new ToolsVM(t)).ToList(),
                Workbench = userInfo.Workbench.Select(t => new ToolsVM(t)).ToList(),
            };
            return View("Profile", userProfile);
        }
        public ActionResult CreateTool()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTool(Tool tool)
        {
            var currentUserId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                currentUser.MyTools.Add(tool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tool);
        }

        public ActionResult EditTool(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tool tool = db.Tools.Find(id);
            if (tool == null)
            {
                return HttpNotFound();
            }
            return View(tool);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTool(Tool tool)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tool);
        }

        public RedirectToRouteResult DeleteTool(int id)
        {
            var tool = db.Tools.Find(id);
            db.Tools.Remove(tool);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}