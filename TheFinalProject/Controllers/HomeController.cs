using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
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

        public ActionResult GeneralView()
        {
            var model = db.Tools.Where(u => u.IsAvailable).ToList().Select(r => new ToolsVm
            {
                Photo = r.Photo,
                Title = r.Title,
                Description = r.Description,
                CategoryName = r.ToolCategory,
                IsAvailable = r.IsAvailable,
                ZipCode = r.ZipCode,
                City = r.City,
                State = r.State
            });

            ViewData["isAvailable"] = true;

            return View(model);
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