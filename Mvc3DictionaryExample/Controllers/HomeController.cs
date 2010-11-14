using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc3DictionaryExample.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            Dictionary<string, int> scores = new Dictionary<string, int>()
            {
                { "John", 90 },
                { "Tom", 49 },
                { "James", 20 },
                { "Chris", 34 },
                { "Aaron", 97 },
            };


            return View(scores);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(Dictionary<string, int> scores)
        {
            return View(scores);
        }
    }
}
