using System;
using System.Linq;
using System.Collections.Generic;
using PagedList;

namespace FirstWeekWorkTest.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{

        客戶聯絡人Repository customerContactRepo;
        客戶資料Repository customerDataRepo;
        客戶銀行資訊Repository customerBankRepo;


        //public 客戶銀行資訊Repository()
        //{


        //}
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(a=>a.是否已刪除==false);
        }



        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }

        public List<CustomerDatastatistics> GetReportData()
        {
            customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();
            customerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();

            var customerDatas = customerDataRepo.All().Select(
              c =>
              new CustomerDatastatistics
              {
                  ID = c.Id,
                  客戶名稱 = c.客戶名稱,
              }).ToList();


            foreach (var a in customerDatas)
            {
                var customerContactDatas = customerContactRepo.All().Where(c => c.客戶Id == a.ID).Count();

                var customerBankDatas = customerBankRepo.All().Where(c => c.客戶Id == a.ID).Count();

                a.聯絡人數量 = customerContactDatas;
                a.銀行帳戶數量 = customerBankDatas;
            }

            return customerDatas;

        }


        public IPagedList<客戶銀行資訊> GetCustomerBankDatasPagedList(string sortOrder, string CurrentSort, int? page)
        {
            // 顯示的表格欄位有幾格
            int pageSize = 10;
            // 一開始的頁面;
            //int pageIndex = 1;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var customerBankDatas = base.All();

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "客戶ID" : sortOrder;

            // Return Data.        
            IPagedList<客戶銀行資訊> emp = null;

            switch (sortOrder)
            {
                case "客戶名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                                (m => m.客戶資料.客戶名稱).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                                (m => m.客戶資料.客戶名稱).ToPagedList(pageIndex, pageSize);
                    break;

                case "銀行名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                                 (m => m.銀行名稱).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                                 (m => m.銀行名稱).ToPagedList(pageIndex, pageSize);
                    break;

                case "銀行代碼":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                                (m => m.銀行代碼).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                               (m => m.銀行代碼).ToPagedList(pageIndex, pageSize);
                    break;

                case "分行代碼":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                                 (m => m.分行代碼).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                                  (m => m.分行代碼).ToPagedList(pageIndex, pageSize);
                    break;
                case "帳戶名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                                 (m => m.帳戶名稱).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                                  (m => m.帳戶名稱).ToPagedList(pageIndex, pageSize);
                    break;

                default:
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerBankDatas.OrderByDescending
                            (m => m.Id).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerBankDatas.OrderBy
                            (m => m.Id).ToPagedList(pageIndex, pageSize);
                    break;
            }

            return emp;

        }
            




        /// <summary>
        /// 分頁搜尋資料
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="CurrentSort"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IPagedList<CustomerDatastatistics> GetStaticsDataPagedList(string sortOrder, string CurrentSort, int? page)
        {

            // 顯示的表格欄位有幾格
            int pageSize = 10;
            // 一開始的頁面;
            //int pageIndex = 1;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var customerDatas = GetReportData();

            sortOrder = String.IsNullOrEmpty(sortOrder) ? "客戶ID" : sortOrder;
           
            // Return Data.        

            //https://www.c-sharpcorner.com/UploadFile/rahul4_saxena/paging-and-sorting-in-mvc4-using-pagedlist/

            IPagedList<CustomerDatastatistics> emp = null;

            switch (sortOrder)
            {
                case "客戶名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                                (m => m.客戶名稱).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerDatas.OrderBy
                                (m => m.客戶名稱).ToPagedList(pageIndex, pageSize);
                    break;

                case "銀行帳戶數量":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                                 (m => m.銀行帳戶數量).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerDatas.OrderBy
                                 (m => m.銀行帳戶數量).ToPagedList(pageIndex, pageSize);
                    break;

                case "聯絡人數量":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                                (m => m.聯絡人數量).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerDatas.OrderBy
                               (m => m.聯絡人數量).ToPagedList(pageIndex, pageSize);
                    break;

                case "客戶ID":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                                 (m => m.ID).ToPagedList(pageIndex, pageSize);
                     else
                       emp = customerDatas.OrderBy
                                 (m => m.ID).ToPagedList(pageIndex, pageSize);
                    break;

                default:
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                            (m => m.ID).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerDatas.OrderBy
                            (m => m.ID).ToPagedList(pageIndex, pageSize);
                    break;
            }
            
            return emp;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}