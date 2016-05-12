using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Humanizer;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using TheFinalProject.Models;
using DbContext = TheFinalProject.Models.DbContext;

namespace TheFinalProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DbContext db = new DbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            return Profile(userId);
        }

        public ActionResult GeneralView(string option, string search )
        {
            
            var userInfo = db.Users.Find(User.Identity.GetUserId());

            var toolsList = new List<Tool>();
           
            if (option == "Title")
            {
                 toolsList = db.Tools.Where(x => x.Title.Contains(search) || search == null).ToList();
            }
            else if (option == "Description")
            {
                toolsList = db.Tools.Where(x => x.Description.Contains(search) || search == null).ToList();
            }
            else if (option == "ZipCode")
            {
                toolsList = db.Tools.Where(x => x.ZipCode.Contains(search) || search == null).ToList();
            }
            else
            {
                toolsList = db.Tools.Where(u => u.IsAvailable).ToList();
            }

          var completeTools = toolsList.Select(r => new ToolsVm
            {
              ToolId = r.Id,
                Photo = r.Photo,
                Title = r.Title,
                Description = r.Description,
                CategoryName = r.ToolCategory,
                IsAvailable = r.IsAvailable,
                ZipCode = r.ZipCode,
                City = r.City,
                State = r.State,
                userId = r.Owner.Id
          });

            var model = new SearchVM()
            {
                 Workbench = userInfo.Workbench.Select(t => new ToolsVm(t)).ToList(),
                 SearchTools = completeTools.ToList()
            };
           
            return View(model);
        }

        [HttpPost]
        public ActionResult AddWorkBench(int Id)
        {
            var tool = db.Tools.FirstOrDefault(x => x.Id == Id);
            var userInfo = db.Users.Find(User.Identity.GetUserId());
            userInfo.Workbench.Add(tool);

            db.SaveChanges();

            return Content("done");
        }

        [HttpPost]
        public ActionResult RemoveWorkBench(int Id)
        {
            var tool = db.Tools.FirstOrDefault(x => x.Id == Id);
            var userInfo = db.Users.Find(User.Identity.GetUserId());
            userInfo.Workbench.Remove(tool);

            db.SaveChanges();
            return Content("done");
        }

      

        public ActionResult Profile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userInfo = db.Users.Find(id);

            var userProfile = new ProfileVM
            {
                Email = userInfo.Email,
                Photo = userInfo.Photo,
                Phone = userInfo.Phone,
                City = userInfo.City,
                State = userInfo.State,
                Zip = userInfo.Zip,
                MyTools = userInfo.MyTools.Select(t => new ToolsVm(t)).ToList(),
                Workbench = userInfo.Workbench.Select(t => new ToolsVm(t)).ToList()
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
            var tool = db.Tools.Find(id);
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