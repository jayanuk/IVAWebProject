using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IVA.FindExpert.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            //var token = await Utility.GetToken("94779827351", "3153");

            return View();
        }

        public ActionResult WelcomeOne()
        {            
            return View("WelcomeOneView");
        }

        public ActionResult WelcomeTwo()
        {           
            return View("WelcomeTwoView");
        }

    }
}
