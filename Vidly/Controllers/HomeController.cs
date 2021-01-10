using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.DBContext;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class HomeController : Controller
    {
        public MyDBContext db = new MyDBContext();

        public ActionResult Index()
        {
            return RedirectToAction("About", "Home", new { name = "hello" });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Profile(string id)
        {
            var res = Server.HtmlEncode($"<h1> requested movie: {id} </h1>");
            ViewBag.name = id;
            return View();
        }

        [Route("Home/movie")]
        public ActionResult Movie()
        {
            List<Movie> movies = new List<Movie>();
            using (MyDBContext DBmodel = new MyDBContext())
            {
                movies = DBmodel.Movies.ToList<Movie>();
            }
            //ViewBag.data = movies;
            return View(movies);
        }

        [Route("Home/Movie/Add")]
        public ActionResult AddMovie()
        {
            return View(new Movie());
        }

        [Route("Home/Movie/Add")]
        [HttpPost]
        public ActionResult AddMovie(Movie movie)
        {
            if(ModelState.IsValid)
            {
                if(movie.Name.Length < 3)
                {
                    ModelState.AddModelError("err", "Name is short");
                    return View(movie);
                }

                //var result = from m in db.Movies where m.Name == movie.Name select m;
                var result = db.Movies.FirstOrDefault( data => data.Name == movie.Name);
                /*result.Any() to check if result has no value,  if result is a list */
                if(result != null)
                {
                    //ModelState.AddModelError("Name", "the name already exist");
                    return Content(result.Name);
                }
                //movie.Name = "hello";
                db.Movies.Add(movie);
                db.SaveChanges();

                return RedirectToAction("movie");
            }
            

            return View(movie);
        }

    }


}