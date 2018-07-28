using FirstWeekWorkTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using FirstWeekWorkTest.Attributes;

namespace FirstWeekWorkTest.Controllers
{
    
    public class CustomerDataStatisticsController : BaseController
    {
        客戶聯絡人Repository customerContactRepo;
        客戶資料Repository customerDataRepo;
        客戶銀行資訊Repository customerBankRepo;
       
        public CustomerDataStatisticsController()
        {
            customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();
            customerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        }


        // 要放在邏輯曾
        // GET: CustomerDataStatistics
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var pageListModle=customerBankRepo.GetStaticsDataPagedList(sortOrder,CurrentSort,page);

            return View(pageListModle);
        }

        



    }  
}