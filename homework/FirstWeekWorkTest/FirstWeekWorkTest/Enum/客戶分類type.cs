using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstWeekWorkTest.Enum
{
    
    public enum 客戶分類type
    {
        [Display(Name = "一般會員")]
        一般會員 = 0,
        黃金會員 = 1,
        白金會員 = 2,
        鑽石會員 = 4,
        終身會員 = 8,
        至尊會員 = 16,
    }
}