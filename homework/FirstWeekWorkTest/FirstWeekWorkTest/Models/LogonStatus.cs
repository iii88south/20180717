using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FirstWeekWorkTest.Models
{
    public class LogonStatus
    {
        public bool isSigned()
        {
            var context = HttpContext.Current;
            if (context.User != null)
            {
                var formIdentify = context.User.Identity as FormsIdentity;
                bool IsAuthenticated = context.User.Identity.IsAuthenticated;
                if (formIdentify != null && IsAuthenticated)
                {
                    return true;
                }
            }
            return false;
        }
    }
}