using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Humanizer;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TheFinalProject.Models;
using DbContext = TheFinalProject.Models.DbContext;
using SendGrid;


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



        [HttpGet]
        public ActionResult SearchView(string zipCode)
        {
            var searchResult = db.Tools.Where(x => x.ZipCode.Contains(zipCode) || zipCode == null);

            return View(searchResult);
        }


        public ActionResult GeneralView(string option, string search, string zipcode)
        {
            //sendgrid begins
        
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
            else
            {
                toolsList = db.Tools.ToList();
            }

            if (zipcode != "")
            {
                toolsList = toolsList.Where(x => x.ZipCode == zipcode).ToList();
            }

            var completeTools = toolsList.Select(r => new ToolsVm
            {
                ToolId = r.Id,
                Photo = "https://toolrental.blob.core.windows.net/toolimages/" + r.Photo,
                Title = r.Title,
                Description = r.Description,
                CategoryName = r.ToolCategory,
                IsAvailable = r.IsAvailable,
                ZipCode = r.ZipCode,
                City = r.City,
                State = r.State,
                UserId = r.Owner.Id,
                UserEmail = r.Owner.Email
            });

            var model = new SearchVM
            {
                Workbench = userInfo.Workbench.Select(t => new ToolsVm(t)).ToList(),
                SearchTools = completeTools.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddtoFollowing(string id)
        {
            var follow = db.Users.FirstOrDefault(x => x.Id == id);

            var followInfo = db.Users.Find(User.Identity.GetUserId());

            followInfo.Following.Add(follow);
            db.SaveChanges();
            return Content("done");
        }

        [HttpPost]
        public ActionResult RemoveFollowing(string id)
        {
            var follow = db.Users.FirstOrDefault(x => x.Id == id);

            var followInfo = db.Users.Find(User.Identity.GetUserId());

            followInfo.Following.Remove(follow);
            db.SaveChanges();
            return Content("done");
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

        [HttpPost]
        public ActionResult Follow(int id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
           
            return Content("done");
        }
      

        public ActionResult Profile(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userInfo = db.Users.Find(id);
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            
            var userProfile = new ProfileVm
            {
                Email = userInfo.Email,
                Photo = userInfo.Photo,
                Phone = userInfo.Phone,
                City = userInfo.City,
                State = userInfo.State,
                Zip = userInfo.Zip,
                MyTools = userInfo.MyTools.Select(t => new ToolsVm(t)).ToList(),
                Workbench = currentUser.Workbench.Select(t => new ToolsVm(t)).ToList(),
                Following =   currentUser.Following.Select (t => new ProfileVm(t)).ToList(),
            };

            return View("Profile", userProfile);
        }
    

        public ActionResult CreateTool()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTool(Tool tool, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                

                var newImageName = UploadImage(photo.InputStream);
                tool.Photo = newImageName;

                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                currentUser.MyTools.Add(tool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tool);
        }

        private static string UploadImage(Stream photo)
        {
            var newImageName = $"{Guid.NewGuid()}.jpg";
            var storageAccount = CloudStorageAccount
                .Parse(
                    "DefaultEndpointsProtocol=https;AccountName=toolrental;AccountKey=5756pti72TndAssLpwfERuzmcFfNaj/b4cgD+339wlntTokGFkZ9bQXP0ua5FlbnZdldoTfa+2TOJDWG5J4J6Q==");

            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("toolimages");

            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});


            var blockBlob = container.GetBlockBlobReference(newImageName);
            blockBlob.UploadFromStream(photo);

            return newImageName;
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
        public ActionResult EditTool(Tool tool, HttpPostedFileBase photo)
        {

            if (ModelState.IsValid)
            {
                if(photo == null)
                {
                    db.SaveChanges();
                    db.Entry(tool).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                var newImageName = UploadImage(photo.InputStream);
                tool.Photo = newImageName;
                db.SaveChanges();
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