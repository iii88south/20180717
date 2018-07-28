using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstWeekWorkTest.Models
{
    public class CustomerDatastatistics
    {
        public CustomerDatastatistics()
        {
           
        }

        public int ID { get; set; }

        public string 客戶名稱 { get; set; }
        [UIHint("ContactCount")]
        public int 聯絡人數量 { get; set; }
        [UIHint("BankCount")]
        public int 銀行帳戶數量 { get; set; }
    }
}