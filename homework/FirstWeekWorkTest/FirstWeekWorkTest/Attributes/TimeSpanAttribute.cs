using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstWeekWorkTest.Attributes
{
    public class TimeSpanAttribute : ActionFilterAttribute
    {

        private Stopwatch stopWatch = new Stopwatch();
       

        // 在執行action之前
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Reset();
            stopWatch.Start();
            Debug.Print("測試執行Action之前訊息");           
            base.OnActionExecuting(filterContext);
            stopWatch.Stop();           
            Debug.Print("測試執行Action之前 總共花費:{0}秒",ts.ToString());

        }

        // 在執行之後
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            TimeSpan ts = stopWatch.Elapsed;
            stopWatch.Reset();
            stopWatch.Start();
            Debug.Print("測試執行Action之前訊息");
            base.OnActionExecuted(filterContext);
            stopWatch.Stop();
            Debug.Print("測試執行Action之前 總共花費:{0}秒", ts.ToString());
        }        
    }
}