using FirstWeekWorkTest.Attributes;
using FirstWeekWorkTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstWeekWorkTest.Controllers
{
   [TimeSpan]
    public class BaseController : Controller
    {     

        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }        
    }
}