using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using FirstWeekWorkTest.Models;
using Omu.ValueInjecter;

namespace FirstWeekWorkTest.Controllers
{
    public class CustomerBankDataController : BaseController
    {
        //private 客戶資料Entities1 db = new 客戶資料Entities1();

        客戶銀行資訊Repository customerBankDatasRepo;
        客戶資料Repository customerDatasRepo;

        
        public CustomerBankDataController()
        {
            customerBankDatasRepo = RepositoryHelper.Get客戶銀行資訊Repository();
            customerDatasRepo = RepositoryHelper.Get客戶資料Repository();
        }

        [HandleError(View = "Error")]
        // GET: CustomerBankData
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var pageListModle = customerBankDatasRepo.GetCustomerBankDatasPagedList(sortOrder, CurrentSort, page);            
            return View(pageListModle);
        }


        [HandleError(View="Error")]
        // GET: CustomerBankData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankDatasRepo.Find(id.Value);
            
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankData/Create
        public ActionResult Create()
        {
            SetCustomerDataDropDown();
            return View();
        }

        // POST: CustomerBankData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [HandleError(View = "Error")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                customerBankDatasRepo.Add(客戶銀行資訊);
                customerBankDatasRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            SetCustomerDataDropDown();
            return View(客戶銀行資訊);
        }

        // GET: CustomerBankData/Edit/5
        [HandleError(View = "Error")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankDatasRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            SetCustomerDataDropDown();
            return View(客戶銀行資訊);
        }

        public void SetCustomerDataDropDown()
        {
            // Must Change! 
            var dictionary = new Dictionary<int, string>();

            var customerIDList = customerDatasRepo.All();

            foreach (var item in customerIDList)
            {
                dictionary.Add(item.Id, item.客戶名稱);
            }
            ViewBag.客戶Id = new SelectList(dictionary, "Key", "Value");
        }

        // POST: CustomerBankData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, 客戶銀行資訊 form)
        {
            var customerdata = customerBankDatasRepo.Find(id);

            if (ModelState.IsValid)
            {
                customerdata.InjectFrom(form);
                customerBankDatasRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            SetCustomerDataDropDown();
            return View(customerdata);
        }

        // GET: CustomerBankData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankDatasRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: CustomerBankData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = customerBankDatasRepo.Find(id);
            customerBankDatasRepo.Delete(客戶銀行資訊);
            customerBankDatasRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerBankDatasRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// EXCEL 輸出客戶銀行資料;
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportCustomerBankDatasExcel()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var data = customerBankDatasRepo.All().Select(c => new
                {
                    c.客戶資料.客戶名稱,
                    c.銀行代碼,
                    c.分行代碼,
                    c.銀行名稱,
                    c.帳戶名稱,
                    c.帳戶號碼,
                }).ToList();

                var ws = wb.Worksheets.Add("cusBankdata", 1);
                ws.Cell(1, 1).InsertData(data);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    // application/vnd.ms-excel
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "客戶銀行資料.xlsx");
                }
            }

        }
    }
}
