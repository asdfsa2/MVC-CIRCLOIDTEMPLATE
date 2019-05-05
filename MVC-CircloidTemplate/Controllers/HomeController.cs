using MVC_CircloidTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVC_CircloidTemplate.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        
        public ActionResult Index()
        {
            ViewBag.OnlineUserCount = HttpContext.Application["OnlineUserCount"];
            ViewBag.TotalUserCount = HttpContext.Application["TotalUserCount"];

            return View();
        }
        public ActionResult CookieAta()
        {
            //Cookiye değer atamayı sağlayacak
            return View();
        }
        [HttpPost]
        public ActionResult CookieAta(string CookieName, string CookiValue)
        {
            HttpCookie ck = new HttpCookie(CookieName);
            ck.Value = CookiValue;
            ck.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(ck);
            return View();
        }
        public ActionResult CookieOku()
        {
            string cv =Request.Cookies["CS-302"].Value;
            ViewBag.CookieSample = cv;
            return View();
        }

    }
}