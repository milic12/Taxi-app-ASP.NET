using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taxi_04.Models;

namespace Taxi_04.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = getRole();

                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();

        }

        public String getRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return "admin";
                }
                else if(s[0].ToString() == "Driver")
                {
                    return "driver";
                }
                else
                {
                    return "passenger";
                }
            }
            return "N/A";
        }


    }
}