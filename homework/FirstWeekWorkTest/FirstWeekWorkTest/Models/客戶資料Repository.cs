using System;
using System.Linq;
using System.Collections.Generic;
using PagedList;
using FirstWeekWorkTest.Helpers;

namespace FirstWeekWorkTest.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {

        public 客戶資料Repository()
        {
                
        }
        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }

        public int findRepeatEmail(string args)
        {
            var count = this.All().Where(p => p.Email == args).Count();
            return count;
        }

        public IPagedList<客戶資料> SearchFilterQuery(string args, string category)
        {            
            var result = this.All();
            if (!string.IsNullOrEmpty(args))
            {
                result = result.Where(p => p.客戶名稱.Contains(args));
            }

            if (!string.IsNullOrEmpty(category))
            {
                int a = Convert.ToInt32(category);
                result = result.Where(c => (int)c.客戶分類 ==a);
            }

            IPagedList<客戶資料> emp = null;
            // pageNum must 1.
            emp = result.OrderByDescending(m => m.Id).ToPagedList(1,10);
            return emp;
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(c => c.是否已刪除 == false);
        }

        public bool ISMember(string account,string password)
        {
            password = new EncodeCryptHelper().Encode(password);
            var count = this.All().Where(s => s.帳號 == account && s.密碼 == password).Count();
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public IPagedList<客戶資料> GetCustomerDatasPagedList(string sortOrder, string CurrentSort, int? page)
        {
            // 顯示的表格欄位有幾格
            int pageSize = 10;
            // 一開始的頁面;
            //int pageIndex = 1;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            客戶資料Repository customerDataRepo;
            customerDataRepo = RepositoryHelper.Get客戶資料Repository();


            var customerDatas = customerDataRepo.All().ToList();
             
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "客戶ID" : sortOrder;

            // Return Data.        

            //https://www.c-sharpcorner.com/UploadFile/rahul4_saxena/paging-and-sorting-in-mvc4-using-pagedlist/

            IPagedList<客戶資料> emp = null;

            switch (sortOrder)
            {
                case "客戶名稱":
                    if (sortOrder.Equals(CurrentSort))
                        emp = customerDatas.OrderByDescending
                                (m => m.Id).ToPagedList(pageIndex, pageSize);
                    else
                        emp = customerDatas.OrderBy
                                (m => m.客戶名稱).ToPagedList(pageIndex, pageSize);
                    break;

                default:
                    emp = customerDatas.OrderByDescending
                               (m => m.Id).ToPagedList(pageIndex, pageSize);
                    break;


            }

            return emp;
        }


    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}