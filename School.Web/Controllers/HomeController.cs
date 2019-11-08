using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.DataAccess.SqlServer;
using School.Entities.ApplicationOrganization;
using School.Entities.GroupOrganization;
using School.ViewModels.ApplicationOrganization;
using School.Web.Models;

namespace School.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataExtension<ApplicationUser> _userExtension;
        private readonly IDataExtension<AnAssociation> _anAssociationExtension;
        private readonly IDataExtension<ActivityTerm> _activityTermExtension;
        public HomeController(IDataExtension<ApplicationUser> userExtension,
            IDataExtension<AnAssociation> anAssociationExtension,
            IDataExtension<ActivityTerm> activityTermExtension)
        {
            _userExtension = userExtension;
            _anAssociationExtension = anAssociationExtension;
            _activityTermExtension = activityTermExtension;
        }

        public async Task<IActionResult> Index()
        {
            var user = User.Claims.FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            var UserInfo = await _userExtension.GetSingleAsyn(x => x.Id == user.Value);
            if (UserInfo.Power == AnJurisdiction.Ordinary)
            {
                return RedirectToAction("Error");
            }
            var userCollection = await _userExtension.GetAll().Include(x => x.Avatar).OrderByDescending(x => x.RegisterTime).ToListAsync();
            var users = userCollection.Take(10).ToList();
            var userList = new List<ApplicationUserVM>();
            foreach (var item in users)
            {
                userList.Add(new ApplicationUserVM(item));
            }
            ViewBag.Users = userCollection.Count();
            ViewBag.AnAssociations =(await _anAssociationExtension.GetAll().ToListAsync()).Count();
            ViewBag.ActivityTerms = (await _activityTermExtension.GetAll().ToListAsync()).Count();
            return PartialView("Index", userList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult School()
        {
            return View();
        }

        public IActionResult NotLogin()
        {
            return View();
        }
    }
}
