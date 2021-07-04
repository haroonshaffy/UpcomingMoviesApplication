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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/");
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
        public ActionResult Associate(int id, int ActorID)
        {
            //Call our API to associate Movie with Actor
            string url = "moviedata/associatemoviewithactor/" + id + "/" + ActorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //GET: Movie/UnAssociate/{id}?ActorID={actorid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ActorID)
        {
            //Call our API to associate Movie with Actor
            string url = "moviedata/unassociatemoviewithactor/" + id + "/" + ActorID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //POST: Movie/AssociateGenre/{movieid}
        [HttpPost]
        public ActionResult AssociateGenre(int id, int GenreID)
        {
            //Call our API to associate Movie with Genre
            string url = "moviedata/associatemoviewithgenre/" + id + "/" + GenreID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //GET: Movie/UnAssociateGenre/{id}?GenreID={genreid}
        [HttpGet]
        public ActionResult UnAssociateGenre(int id, int GenreID)
        {
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
        public ActionResult New()
        {
            /*
            //Information about all genres in the system
            //GET api/genredata/listgenre

            string url = "genredata/listgenre";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<Genre> GenreList = response.Content.ReadAsAsync<IEnumerable<Genre>>().Result;

            return View(GenreList);*/
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
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
        public ActionResult Edit(int id, Movie movie)
        {
            //Objective : Add a new movie into our system using the API
            //curl -H "Content-type:application/json" -d @movie.json https://localhost:44343/api/moviedata/addmovie
            string url = "moviedata/updatemovie/" + id;

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
            string url = "moviedata/findmovie/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            MovieDto selectedmovie = response.Content.ReadAsAsync<MovieDto>().Result;

            return View(selectedmovie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Movie movie)
        {
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