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
    public class MovieController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MovieController()
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

        // GET: Movie/List
        public ActionResult List()
        {
            //Objective: To communicate with the Movie data API to retrieve the list of movies
            //curl https://localhost:44343/api/moviedata/listmovies

            string url = "moviedata/listmovies";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> movies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            DetailsMovie ViewModel = new DetailsMovie();

            //Objective: To communicate with the Movie data API to retrieve one movie
            //curl https://localhost:44343/api/moviedata/findmovie/{id}

            string url = "moviedata/findmovie/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto SelectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;

            Debug.WriteLine(SelectedMovie.MovieHasPic);
            ViewModel.SelectedMovie = SelectedMovie;

            //Show associated actors starring in this movie
            url = "actordata/listactorsformovie/"+id;
            response = client.GetAsync(url).Result;
            IEnumerable <ActorDto> ActorsForMovie = response.Content.ReadAsAsync<IEnumerable<ActorDto>>().Result;

            ViewModel.ActorsForMovie = ActorsForMovie;

            //Show associated genres for this movie
            url = "genredata/listgenresformovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GenreDto> GenresForMovie = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;

            ViewModel.GenresForMovie = GenresForMovie;

            //Show genres not associated with a particular movie
            url = "genredata/listgenresnotformovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GenreDto> GenresNotForMovie = response.Content.ReadAsAsync<IEnumerable<GenreDto>>().Result;

            ViewModel.GenresNotForMovie = GenresNotForMovie;


            //Show actors not associated with a particular movie
            url = "actordata/ListActorsNotInMovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ActorDto> ActorsNotInMovie = response.Content.ReadAsAsync<IEnumerable<ActorDto>>().Result;

            ViewModel.ActorsNotInMovie = ActorsNotInMovie;

            return View(ViewModel);
        }

        //POST: Movie/Associate/{movieid}
        [HttpPost]
        [Authorize]
        public ActionResult Associate(int id, int ActorID)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Call our API to associate Movie with Actor
            string url = "moviedata/associatemoviewithactor/" + id + "/" + ActorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Movie/UnAssociate/{id}?ActorID={actorid}
        [HttpGet]
        [Authorize]
        public ActionResult UnAssociate(int id, int ActorID)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Call our API to associate Movie with Actor
            string url = "moviedata/unassociatemoviewithactor/" + id + "/" + ActorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //POST: Movie/AssociateGenre/{movieid}
        [HttpPost]
        [Authorize]
        public ActionResult AssociateGenre(int id, int GenreID)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Call our API to associate Movie with Genre
            string url = "moviedata/associatemoviewithgenre/" + id + "/" + GenreID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: Movie/UnAssociateGenre/{id}?GenreID={genreid}
        [HttpGet]
        [Authorize]
        public ActionResult UnAssociateGenre(int id, int GenreID)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Call our API to associate Movie with Actor
            string url = "moviedata/unassociatemoviewithgenre/" + id + "/" + GenreID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        public ActionResult Error()
        {
            return View();
        }

        // GET: Movie/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Movie movie)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective : Add a new movie into our system using the API
            //curl -H "Content-type:application/json" -d @movie.json https://localhost:44343/api/moviedata/addmovie

            string jsonpayload = jss.Serialize(movie);

            string url = "moviedata/addmovie";

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
        [Authorize]
        public ActionResult Update(int id)
        {
            //Find the movie to show to the user to understand what is being edited

            string url = "moviedata/findmovie/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto selectedmovie = response.Content.ReadAsAsync<MovieDto>().Result;

            return View(selectedmovie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Movie movie, HttpPostedFileBase MoviePic)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective : Add a new movie into our system using the API
            //curl -H "Content-type:application/json" -d @movie.json https://localhost:44343/api/moviedata/addmovie
            string url = "moviedata/updatemovie/" + id;

            string jsonpayload = jss.Serialize(movie);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && MoviePic != null)
            {
                //Updating the movie picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "MovieData/UploadMoviePic/" + id;
                //Debug.WriteLine("Received Movie Picture "+MoviePicPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(MoviePic.InputStream);
                requestcontent.Add(imagecontent, "MoviePic", MoviePic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }

            else if (response.IsSuccessStatusCode)
            {
                //No image upload but update still successful

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "moviedata/findmovie/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto selectedmovie = response.Content.ReadAsAsync<MovieDto>().Result;

            return View(selectedmovie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, Movie movie)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            string url = "moviedata/deletemovie/" + id;

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