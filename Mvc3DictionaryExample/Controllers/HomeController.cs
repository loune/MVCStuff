using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc3DictionaryExample.Models;

namespace Mvc3DictionaryExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SimpleTest()
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
        public ActionResult SimpleTest(Dictionary<string, int> scores)
        {
            return View(scores);
        }

        public ActionResult GuidTest()
        {
            Dictionary<Guid, int> scores = new Dictionary<Guid, int>()
            {
                { Guid.NewGuid(), 90 },
                { Guid.NewGuid(), 49 },
                { Guid.NewGuid(), 20 },
                { Guid.NewGuid(), 34 },
                { Guid.NewGuid(), 97 },
            };


            return View(scores);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GuidTest(Dictionary<Guid, int> scores)
        {
            return View(scores);
        }

        public ActionResult ComplexTest()
        {
            Dictionary<int, Person> people = new Dictionary<int, Person>()
            {
                { 1, new Person() { FirstName = "John", LastName = "Smith", Age = 30, Location = "Sydney" } },
                { 2, new Person() { FirstName = "Bob", LastName = "Hawke", Age = 30, Location = "Sydney" } },
                { 3, new Person() { FirstName = "Bob", LastName = "Alice", Age = 30, Location = "Hobart" } },
                { 4, new Person() { FirstName = "Tom", LastName = "Bob", Age = 30, Location = "Melbourne" } },
                { 5, new Person() { FirstName = "Paul", LastName = "Smith", Age = 30, Location = "Perth" } },
            };


            return View(people);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ComplexTest(Dictionary<int, Person> people)
        {
            return View(people);
        }
    }
}
