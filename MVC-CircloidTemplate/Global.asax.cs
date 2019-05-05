using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_CircloidTemplate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            if (Application["OnlineUserCount"] == null)
            {
                int counter = 1;
                Application["OnlineUserCount"] = counter;
            }
            else
            {
                int counter = (int)Application["OnlineUserCount"];
                counter++;
                Application["OnlineUserCount"] = counter;
            }

            if (Application["TotalUserCount"] == null)
            {
                int counter = 1;
                Application["TotalUserCount"] = counter;
            }
            else
            {
                int counter = (int)Application["TotalUserCount"];
                counter++;
                Application["TotalUserCount"] = counter;

            }
        }
        protected void Session_End()
        {
            int counter = (int)Application["OnlineUserCount"];
            counter--;
            Application["OnlineUserCount"] = counter;


        }

    }
}
