using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using UpcomingMoviesApplication.Models;
using UpcomingMoviesApplication.Models.ViewModels;
using System.Web.Script.Serialization;

namespace UpcomingMoviesApplication.Controllers
{
    public class GenreController : Controller
    {
        public static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GenreController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44343/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        // GET: Genre/List
        public ActionResult List()
        {
            //Objective: Communicate with Genre data API to retrieve a list of genres
            //curl https://localhost:44343/api/genredata/listgenres

            string url = "genredata/listgenres";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<GenreDto> genres = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;


            return View(genres);
        }

        // GET: Genre/Details/5
        public ActionResult Details(int id)
        {
            DetailsGenre ViewModel = new DetailsGenre();
            //Objective: Communicate with Genre data API to retrieve one genre
            //curl https://localhost:44343/api/genredata/findgenre/{id}

            string url = "genredata/findgenre/"+id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            GenreDto SelectedGenre = response.Content.ReadAsAsync<GenreDto>().Result;

            ViewModel.SelectedGenre = SelectedGenre;

            //Show all movies in this particular genre

            url = "moviedata/listmoviesforgenre/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> MoviesForGenre = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.MoviesForGenre = MoviesForGenre;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Genre/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Genre/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Genre genre)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective: Add a new Genre to our system using the API
            //curl -H "Content-type:application/json" -d @genre.json https://localhost:44343/api/genredata/addgenre

            string jsonpayload = jss.Serialize(genre);

            string url = "genredata/addgenre";

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
        [Authorize]
        public ActionResult Update(int id)
        {
            //Find the genre to show to the user to understand what is being updated

            string url = "genredata/findgenre/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            GenreDto selectedgenre = response.Content.ReadAsAsync<GenreDto>().Result;

            return View(selectedgenre);
        }

        // POST: Genre/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Genre genre)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective : Add a new genre into our system using the API
            //curl -H "Content-type:application/json" -d @genre.json https://localhost:44343/api/genredata/addgenre
            string url = "genredata/updategenre/" + id;

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
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "genredata/findgenre/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            GenreDto selectedgenre = response.Content.ReadAsAsync<GenreDto>().Result;

            return View(selectedgenre);
        }

        // POST: Genre/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, Genre genre)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            string url = "genredata/deletegenre/" + id;

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
