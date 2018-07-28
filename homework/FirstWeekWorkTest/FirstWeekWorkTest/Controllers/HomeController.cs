using FirstWeekWorkTest.Attributes;
using FirstWeekWorkTest.Enum;
using FirstWeekWorkTest.Models;
using FirstWeekWorkTest.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FirstWeekWorkTest.Controllers
{
    [RoutePrefix("Home")]   
    public class HomeController : BaseController
    {
        LogonStatus _LogonStatus = new LogonStatus();
        客戶資料Repository customerdatasRepo;

        public HomeController()
        {
            customerdatasRepo = RepositoryHelper.Get客戶資料Repository();
        }
              
        public ActionResult Index()
        {
            if (_LogonStatus.isSigned())
            {
                ViewBag.User = User.Identity.Name;
                return View();
            }
            return RedirectToAction("Login");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="LogonCollection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(FormCollection LogonCollection, CustomerDataVM model)
        {     
            if (ModelState.IsValid)
            {
                // clear Session now.
                Session.RemoveAll();

                // FormsAuthenticationTicket.
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, model.帳號,
                                                        DateTime.Now, DateTime.Now.AddMinutes(30), false,
                                                        "Member",
                                                        FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                return RedirectToAction("Index");
            }
            return View();
        }
            



        public ActionResult Logout()
        {
            // 移除瀏覽器的表單驗證票證.
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }


    }
}