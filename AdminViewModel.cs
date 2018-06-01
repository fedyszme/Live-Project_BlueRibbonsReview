using BlueRibbonsReview.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlueRibbonsReview.ViewModels
{
    public class AdminViewModel
    {
        /*public ApplicationUser ApplicationUser { get; set; }
        public Campaign Campaign { get; set; }*/
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [DisplayName("Join Date")]
        public DateTime JoinDate { get; set; }
        [DisplayName("Campaign Count")]
        public int CampaignCount { get; set; }
        [DisplayName("Referral Count")]
        public int ReferralCount { get; set; }
        [DisplayName("User Role")]
        public string UserRoleName { get; set; }
        [DisplayName("User ID")]
        public string UserID { get; set; }

        public AdminViewModel()//Need a parameterless constructor for the AdminController Edit - Post to function correctly
        {

        }

        public List<Campaign> Campaigns
        {
            get
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var campaigns = from c in db.Campaigns
                                where c.ApplicationUser.Id == UserID
                                select c;
                return campaigns.ToList<Campaign>();
            }
        }




        public AdminViewModel(ApplicationUser appUser, int campaignCount, int referralCount, string roleName)
        {
            FirstName = appUser.FirstName;
            LastName = appUser.LastName;
            Email = appUser.Email;
            JoinDate = appUser.JoinDate;

            CampaignCount = campaignCount;
            ReferralCount = referralCount;
            UserRoleName = roleName;
            UserID = appUser.Id;
        }
    }
}
