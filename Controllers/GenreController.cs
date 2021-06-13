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
    public class GenreController : Controller
    {
        public static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GenreController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/genredata/");
        }
        // GET: Genre/List
        public ActionResult List()
        {
            //Objective: Communicate with Genre data API to retrieve a list of genres
            //curl https://localhost:44343/api/genredata/listgenre

            string url = "listgenre";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Genre> genre = response.Content.ReadAsAsync<IEnumerable<Genre>>().Result;


            return View(genre);
        }

        // GET: Genre/Details/5
        public ActionResult Details(int id)
        {
            //Objective: Communicate with Genre data API to retriev one genre
            //curl https://localhost:44343/api/genredata/findgenre/{id}

            string url = "findgenre/"+id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Genre selectedgenre = response.Content.ReadAsAsync<Genre>().Result;

            return View(selectedgenre);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Genre/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Genre/Create
        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            //Objective: Add a new Genre to our system using the API
            //curl -H "Content-type:application/json" -d @genre.json https://localhost:44343/api/genredata/addgenre

            string url = "addgenre";

            string jsonpayload = jss.Serialize(genre);

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

        // GET: Genre/Update/5
        public ActionResult Update(int id)
        {
            //Find the genre to show to the user to understand what is being updated

            string url = "findgenre/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Genre selectedgenre = response.Content.ReadAsAsync<Genre>().Result;

            return View(selectedgenre);
        }

        // POST: Genre/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Genre genre)
        {
            //Objective : Add a new genre into our system using the API
            //curl -H "Content-type:application/json" -d @genre.json https://localhost:44343/api/genredata/addgenre
            string url = "updategenre/" + id;

            string jsonpayload = jss.Serialize(genre);

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

        // GET: Genre/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "findgenre/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Genre selectedgenre = response.Content.ReadAsAsync<Genre>().Result;

            return View(selectedgenre);
        }

        // POST: Genre/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Genre genre)
        {
            string url = "deletegenre/" + id;

            string jsonpayload = jss.Serialize(genre);

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
