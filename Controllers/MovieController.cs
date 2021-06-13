using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using UpcomingMoviesApplication.Models;
using System.Web.Script.Serialization;

namespace UpcomingMoviesApplication.Controllers
{
    public class MovieController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MovieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/moviedata/");
        }

        // GET: Movie/List
        public ActionResult List()
        {
            //Objective: To communicate with the Movie data API to retrieve the list of movies
            //curl https://localhost:44343/api/moviedata/listmovies

            string url = "listmovies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Movie> movies = response.Content.ReadAsAsync<IEnumerable<Movie>>().Result;

            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            //Objective: To communicate with the Movie data API to retrieve one movie
            //curl https://localhost:44343/api/moviedata/findmovie/{id}

            string url = "findmovie/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Movie selectedmovie = response.Content.ReadAsAsync<Movie>().Result;

            return View(selectedmovie);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Movie/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            //Objective : Add a new movie into our system using the API
            //curl -H "Content-type:application/json" -d @movie.json https://localhost:44343/api/moviedata/addmovie

            string jsonpayload = jss.Serialize(movie);

            string url = "addmovie";

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Update/5
        public ActionResult Update(int id)
        {
            //Find the movie to show to the user to understand what is being edited

            string url = "findmovie/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Movie selectedmovie = response.Content.ReadAsAsync<Movie>().Result;

            return View(selectedmovie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Movie movie)
        {
            //Objective : Add a new movie into our system using the API
            //curl -H "Content-type:application/json" -d @movie.json https://localhost:44343/api/moviedata/addmovie
            string url = "updatemovie/" + id;

            string jsonpayload = jss.Serialize(movie);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "findmovie/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Movie selectedmovie = response.Content.ReadAsAsync<Movie>().Result;

            return View(selectedmovie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Movie movie)
        {
            string url = "deletemovie/" + id;

            string jsonpayload = jss.Serialize(movie);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
