using ClosedXML.Excel;
using FirstWeekWorkTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace FirstWeekWorkTest.Controllers
{
    public class CustomerContactController : BaseController
    {
        客戶聯絡人Repository customerContactRepo;
        客戶資料Repository customerDataRepo;

        public CustomerContactController()
        {
            customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();
        }

        // GET: CustomerContact
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var pageListModle = customerContactRepo.GetCustomerContactDataPagedList(sortOrder, CurrentSort, page,string.Empty,string.Empty);
            ViewBag.JobTitlesItems = SetJobTitleItems();
            return View(pageListModle);
        }
        
        public ActionResult SearchForCustomerContact(string SearchContactName, string SearchJobTitle)
        {           
            var pageListModle = customerContactRepo.GetCustomerContactDataPagedList(string.Empty, string.Empty, null, SearchContactName, SearchJobTitle);
            ViewBag.JobTitlesItems = SetJobTitleItems();
            return View("Index",pageListModle);            
        }

        public List<SelectListItem> SetJobTitleItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "請選擇", Value = "" });
            var category = customerContactRepo.All().Select(c => new SelectListItem { Text = c.職稱.ToString(), Value = c.職稱.ToString() }).Distinct().ToList();

            foreach (var item in category)
            {
                items.Add(item);
            }
            return items;
        }

        //輸出excel       
        public ActionResult CusContactExport()
        {
            //ClosedXML的用法 先new一個Excel Workbook
            using (XLWorkbook wb = new XLWorkbook())
            {
                //取得我要塞入Excel內的資料
                var data = customerContactRepo.All().Select(c => new { c.客戶Id, c.姓名, c.手機, c.職稱 });

                //一個wrokbook內至少會有一個worksheet,並將資料Insert至這個位於A1這個位置上
                var ws = wb.Worksheets.Add("cusdata", 1);
                //注意官方文件上說明,如果是要塞入Query後的資料該資料一定要變成是data.AsEnumerable()
                //但是我查詢出來的資料剛好是IQueryable ,其中IQueryable有繼承IEnumerable 所以不需要特別寫
                ws.Cell(1, 1).InsertData(data);

                //因為是用Query的方式,這個地方要用串流的方式來存檔
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    //請注意 一定要加入這行,不然Excel會是空檔
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //注意Excel的ContentType,是要用這個"application/vnd.ms-excel" 不曉得為什麼網路上有的Excel ContentType超長,xlsx會錯 xls反而不會
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "客戶聯絡人資料.xlsx");
                }
            }
        }

        
        // GET: CustomerContact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 customerContact = customerContactRepo.Find(id.Value);
            if (customerContact == null)
            {
                return HttpNotFound();
            }
            return View(customerContact);
        }

        // GET: CustomerContact/Create
        public ActionResult Create()
        {
            var a = customerDataRepo.All();
            ViewBag.客戶Id = new SelectList(a, "Id", "客戶名稱", "客戶名稱");
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerContact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                customerContactRepo.Add(客戶聯絡人);
                customerContactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            var a = customerDataRepo.All();
            ViewBag.客戶Id = new SelectList(a, "Id", "客戶名稱", 客戶聯絡人.客戶Id);            
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            var a = customerDataRepo.All();
            ViewBag.客戶Id = new SelectList(a, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = customerContactRepo.UnitOfWork.Context;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var a = customerDataRepo.All();

            ViewBag.客戶Id = new SelectList(a, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id);
            customerContactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerContactRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 批次新增;
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        public ActionResult BatchUpdate(IList<CustomerContactVM> datas)
        {
            //ps: 記得在web config加上<customErrors mode="On"></customErrors>
            if (ModelState.IsValid)
            {
                foreach (var items in datas)
                {
                    var data = customerContactRepo.Find(items.Id);
                    data.Email = items.Email;
                    data.手機 = items.手機;
                    data.電話 = items.電話;
                }
                var db = customerContactRepo.UnitOfWork.Context;            
                db.SaveChanges();
            }
            ViewData.Model = customerContactRepo.AllData();
            ViewBag.JobTitlesItems = SetJobTitleItems();
            return RedirectToAction("Index");

        }
    }
}
