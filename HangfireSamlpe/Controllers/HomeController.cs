using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;


namespace HangfireSamlpe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Hangfireでjobをキューに登録
            var id = BackgroundJob.Enqueue(() => Console.WriteLine("Simple Job"));
            BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));
            return View();
        }
    }
}
