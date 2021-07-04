﻿using System;
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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/");
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
        public ActionResult Edit(int id, Genre genre)
        {
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
        public ActionResult Delete(int id)
        {
            string url = "genredata/findgenre/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            GenreDto selectedgenre = response.Content.ReadAsAsync<GenreDto>().Result;

            return View(selectedgenre);
        }

        // POST: Genre/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Genre genre)
        {
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
