using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using FirstWeekWorkTest.Attributes;
using FirstWeekWorkTest.Enum;
using FirstWeekWorkTest.Helpers;
using FirstWeekWorkTest.Models;

namespace FirstWeekWorkTest.Controllers
{
   
    public class CustomerDatasController : BaseController
    {
        客戶聯絡人Repository customerContactRepo;
        客戶資料Repository customerdatasRepo;

        public CustomerDatasController()
        {
            customerdatasRepo = RepositoryHelper.Get客戶資料Repository();
            customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
        }

        // GET: CustomerDatas
        public ActionResult Index(string sortOrder, string CurrentSort, int? page)
        {            
            ViewBag.CurrentSort = sortOrder;
            var pageListModle = customerdatasRepo.GetCustomerDatasPagedList(sortOrder, CurrentSort, page);
            SetCategoryDDLListItem();
            return View(pageListModle);
        }

        public void SetCategoryDDLListItem()
        {
            // Must Change! 
            var dictionary = new Dictionary<int, string>();

            var category = customerdatasRepo.All().Select(c => c.客戶分類).Distinct().ToList();

            foreach (var item in category)
            {
                //dictionary.Add(item, ((客戶分類type)item).ToString());        
                dictionary.Add((int)item, ((客戶分類type)item).ToString());
            }
            ViewBag.category = new SelectList(dictionary, "Key", "Value");
        }


        public ActionResult SearchForCustomerName(string SearchForCustomer, string Category)
        {
            var customerdata = customerdatasRepo.SearchFilterQuery(SearchForCustomer, Category);
            SetCategoryDDLListItem();
            return View("Index", customerdata);
        }


        /// <summary>
        ///  Partial Customer Data 底下的聯絡人資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details_CustomerContactList(int id)
        {
            ViewData.Model = customerContactRepo.FindFromCustomerID(id);
            // Ienumable;
            return PartialView("CustomerContactList");
        }





        // GET: CustomerDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 customerData = customerdatasRepo.Find(id.Value);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // GET: CustomerDatas/Create
        public ActionResult Create()
        {
            SetCategoryDDLListItem();
            return View();
        }

        // POST: CustomerDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {               
                客戶資料.密碼= new EncodeCryptHelper().Encode(客戶資料.密碼);
                customerdatasRepo.Add(客戶資料);
                customerdatasRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            SetCategoryDDLListItem();
            return View(客戶資料);
        }

        // GET: CustomerDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            SetCategoryDDLListItem();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Use Id.Value cause id?
            客戶資料 customerData = customerdatasRepo.Find(id.Value);
            if (customerData == null)
            {
                return HttpNotFound();
            }
            return View(customerData);
        }

        // POST: CustomerDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, 客戶資料 form)
        {
            var customerdata = customerdatasRepo.Find(id);

            string oldPwd = customerdata.密碼;
            string encryptNewPwd = new EncodeCryptHelper().Encode(form.密碼);
            string NewPwd = form.密碼;
            // TryUpdataModel. Exclude       
            // 利用 TryUpdateModel 來繫結我們要的資料，用這樣的方式就可以避免資料繫結的時候更新到我們不要的資訊，也可以避免一些安全性的問題。
            // Q:繫結的資料是從哪邊來的? 
            // A:預設它會從表單接收到的資料，只要名稱一樣就會自己繫結

            if (TryUpdateModel(customerdata, "", new string[] { "密碼", "電話", "傳真", "地址", "Email" }, new string[] {}))
            {
                if ((oldPwd!= encryptNewPwd)&&(oldPwd != NewPwd))
                {
                    customerdata.密碼 = encryptNewPwd;
                }                
                customerdatasRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            SetCategoryDDLListItem();  
            return View(customerdata);            
        }

        // GET: CustomerDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerdatasRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: CustomerDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 customerData = customerdatasRepo.Find(id);
            customerdatasRepo.Delete(customerData);
            customerdatasRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerdatasRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// expoert 客戶資料 excel.
        /// </summary>
        /// <returns></returns>
        public ActionResult CusDataExport()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var data = customerdatasRepo.All().Select(c => new {
                    c.Id,
                    c.客戶名稱,
                    客戶分類=c.客戶分類.ToString(),
                    c.統一編號,
                    c.電話,
                    c.傳真,
                    c.地址,
                    c.Email
                }).ToList();

                var ws = wb.Worksheets.Add("cusdata", 1);
                ws.Cell(1, 1).InsertData(data);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    // application/vnd.ms-excel
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "客戶資料.xlsx");
                }
            }
        }

        



    }
}
