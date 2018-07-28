using System;
using System.Linq;
using System.Collections.Generic;
using PagedList;

namespace FirstWeekWorkTest.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public bool IsRepeatEMail(int id,int CustomerId,string CustomerEmail)
        {
            var _count = 0;            
            // 新增
            if (id == 0)
            {
                _count=this.All().Where(p => p.客戶Id == CustomerId && p.是否已刪除 == false && p.Email == CustomerEmail).Count();
            }
            else
            {
                var _originalEmail= this.All().Where(p => p.Id==id).SingleOrDefault().Email;
                if (_originalEmail != CustomerEmail)
                {
                    _count = this.All().Where(p => p.客戶Id == CustomerId && p.是否已刪除 == false && p.Email == CustomerEmail).Count();
                }
                else
                {
                    _count = 0;
                }
            }

            if (_count > 0)
            {
                return true;
            }

            return false;
        }

        public List<客戶聯絡人>  FindFromCustomerID(int id)
        {
            return this.AllData().Where(p => p.客戶資料.Id == id).ToList();
        }


        public IPagedList<客戶聯絡人> GetCustomerContactDataPagedList(string sortOrder, string CurrentSort, int? page,string SearchContactName,string SearchJobTitle)
        {
            // 顯示的表格欄位有幾格
            int pageSize = 10;
            // 一開始的頁面;
            //int pageIndex = 1;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var customerContactDatasList = base.All().ToList();

            if (!string.IsNullOrEmpty(SearchContactName))
            {

                customerContactDatasList = customerContactDatasList.Where(a => a.姓名.Contains(SearchContactName)).ToList();

            }

            if (!string.IsNullOrEmpty(SearchJobTitle))
            {
                customerContactDatasList = customerContactDatasList.Where(a => a.職稱 == SearchJobTitle).ToList();
            }

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "客戶ID" : sortOrder;

            // Return Data.        
            IPagedList<客戶聯絡人> emp = null;
            
                switch (sortOrder)
                {
                    case "客戶名稱":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                    (m => m.客戶資料.客戶名稱).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                    (m => m.客戶資料.客戶名稱).ToPagedList(pageIndex, pageSize);
                        break;

                    case "職稱":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                     (m => m.職稱).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                     (m => m.職稱).ToPagedList(pageIndex, pageSize);
                        break;

                    case "姓名":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                    (m => m.姓名).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                   (m => m.姓名).ToPagedList(pageIndex, pageSize);
                        break;

                    case "Email":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                     (m => m.Email).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                      (m => m.Email).ToPagedList(pageIndex, pageSize);
                        break;
                    case "手機":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                     (m => m.手機).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                      (m => m.手機).ToPagedList(pageIndex, pageSize);
                        break;
                    case "電話":
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                     (m => m.電話).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                      (m => m.電話).ToPagedList(pageIndex, pageSize);
                        break;

                    default:
                        if (sortOrder.Equals(CurrentSort))
                            emp = customerContactDatasList.OrderByDescending
                                (m => m.Id).ToPagedList(pageIndex, pageSize);
                        else
                            emp = customerContactDatasList.OrderBy
                                (m => m.Id).ToPagedList(pageIndex, pageSize);
                        break;
                }
            return emp;

        }

        
        public List<客戶聯絡人> AllData()
        {

            客戶聯絡人Repository customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();

            var customerData = RepositoryHelper.Get客戶資料Repository().All().ToList();

            var customerContact = customerContactRepo.All().ToList()
                .Join(customerData, a => a.客戶Id, b => b.Id, (a, b) => new 客戶聯絡人
                {
                    Id = a.Id,
                    客戶Id = a.客戶Id,
                    職稱 = a.職稱,
                    姓名 = a.姓名,
                    Email = a.Email,
                    手機 = a.手機,
                    電話 = a.電話,
                    是否已刪除 = a.是否已刪除,
                    客戶資料 = new 客戶資料 { Id = a.客戶Id, 客戶名稱 = b.客戶名稱 }
                }).Where(c => c.是否已刪除 == false).ToList();
            return customerContact;

        }
        
    }

    public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}