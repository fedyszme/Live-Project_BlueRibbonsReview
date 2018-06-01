using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlueRibbonsReview.Models;
using BlueRibbonsReview.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlueRibbonsReview.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.JoinDateSortParm = String.IsNullOrEmpty(sortOrder) ? "joinDate_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewBag.CampaignCountSortParm = sortOrder == "campaignCount" ? "campaignCount_desc" : "campaignCount";
            ViewBag.ReferralCountSortParm = sortOrder == "referralCount" ? "referralCount_desc" : "referralCount";
            ViewBag.UserRoleSortParm = sortOrder == "userRole" ? "userRole_desc" : "userRole";

            List<AdminViewModel> AdminViewModels = PopulateAdminViewModels();

            if (!String.IsNullOrEmpty(searchString))
            {
                AdminViewModels = AdminViewModels.Where(u => u.LastName == searchString || u.FirstName == searchString).ToList();
            }

            // Default sort order is by join date ascending
            switch (sortOrder)
            {
                case "joinDate_desc":
                    AdminViewModels = AdminViewModels.OrderByDescending(u => u.JoinDate).ToList();
                    break;
                case "campaignCount":
                    AdminViewModels = AdminViewModels.OrderBy(u => u.CampaignCount).ToList();
                    break;
                case "campaignCount_desc":
                    AdminViewModels = AdminViewModels.OrderByDescending(u => u.CampaignCount).ToList();
                    break;
                case "lastName":
                    AdminViewModels = AdminViewModels.OrderBy(u => u.LastName).ToList();
                    break;
                case "lastName_desc":
                    AdminViewModels = AdminViewModels.OrderByDescending(u => u.LastName).ToList();
                    break;
                case "referralCount":
                    AdminViewModels = AdminViewModels.OrderBy(u => u.ReferralCount).ToList();
                    break;
                case "referralCount_desc":
                    AdminViewModels = AdminViewModels.OrderByDescending(u => u.ReferralCount).ToList();
                    break;
                case "userRole":
                    AdminViewModels = AdminViewModels.OrderBy(u => u.UserRoleName).ToList();
                    break;
                case "userRole_desc":
                    AdminViewModels = AdminViewModels.OrderByDescending(u => u.UserRoleName).ToList();
                    break;
                default:
                    AdminViewModels = AdminViewModels.OrderBy(u => u.JoinDate).ToList();
                    break;
            }
            return View(AdminViewModels);
        }


        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<AdminViewModel> AdminViewModels = PopulateAdminViewModels();
            var adminDetails = AdminViewModels.Find(x => x.UserID == id);
            if (adminDetails == null)
            {
                return HttpNotFound();
            }
            return View(adminDetails);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<AdminViewModel> AdminViewModels = PopulateAdminViewModels();
            var adminDetails = AdminViewModels.Find(x => x.UserID == id);
            if (adminDetails == null)
            {
                return HttpNotFound();
            }
            return View(adminDetails);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminViewModel adminViewModel)
        {
            if (adminViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<ApplicationUser> ApplicationUsers = db.Users.ToList(); //Access the Application Users from the database
            var applicationUser = ApplicationUsers.Find(x => x.Id == adminViewModel.UserID); //Finds the specific ApplicationUser that matches Ids from the AdminViewModel

            if (applicationUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            //This updates the ApplicationUser with the values from the form from the AdminViewModel
            applicationUser.FirstName = adminViewModel.FirstName;
            applicationUser.LastName = adminViewModel.LastName;
            applicationUser.Email = adminViewModel.Email;
            applicationUser.JoinDate = adminViewModel.JoinDate;

            if (TryUpdateModel(applicationUser, "", new string[] { "FirstName", "LastName", "Email", "JoinDate" }))
            {
                try
                {
                    db.Entry(applicationUser).State = EntityState.Modified;//this updates the ApplicationUser in the database
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again and if the problem continues contact your System Administrator.");
                }
            }
            return View(adminViewModel);
        }


        // GET: Admin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<AdminViewModel> AdminViewModels = PopulateAdminViewModels();
            var adminDetails = AdminViewModels.Find(x => x.UserID == id);
            if (adminDetails == null)
            {
                return HttpNotFound();
            }
            return View(adminDetails);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {         
            List<ApplicationUser> ApplicationUsers = db.Users.ToList(); //Access the Application Users from the database
            var applicationUser = ApplicationUsers.Find(x => x.Id == id); //Finds the specific ApplicationUser that matches Ids from the AdminViewModel

            if (applicationUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
          
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Export Email List to .csv
        public void ExportEmailListToCSV()
        {
            var emails = from e in db.Users select e;

            StringWriter sw = new StringWriter();

            sw.WriteLine("\"Email\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=Exported_Emails.csv");
            Response.ContentType = "text/csv";

            foreach (var item in emails)
            {
                sw.WriteLine(string.Format("\"{0}\"",
                                           item.Email));
            }

            Response.Write(sw.ToString());

            Response.End();

        }

        // Function to populate a list of AdminViewModels with a ViewModel for each User
        public List<AdminViewModel> PopulateAdminViewModels()
        {
            List<AdminViewModel> AdminViewModels = new List<AdminViewModel>();
            IEnumerable<ApplicationUser> ApplicationUsers = db.Users.ToList();
            foreach (ApplicationUser user in ApplicationUsers)
            {
                string roleName = "";
                foreach (var role in user.Roles)
                {
                    roleName = db.Roles.First(u => u.Id == role.RoleId).Name;
                }
                var campaignCount = db.Campaigns.Count(c => c.ApplicationUser.Id == user.Id);
                var referralCount = db.Referrals.Count(r => r.ApplicationUser.Id == user.Id);
                AdminViewModel AdminUser = new AdminViewModel(user, campaignCount, referralCount, roleName);
                AdminViewModels.Add(AdminUser);
            }
            return AdminViewModels;
        }
    }
}
