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
    public class ActorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ActorController()
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

        // GET: Actor/List
        public ActionResult List()
        {
            //Objective: To communicate with the Actor data API to retrieve the list of Actors
            //curl https://localhost:44343/api/actordata/listactors

            string url = "actordata/listactors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ActorDto> actors = response.Content.ReadAsAsync<IEnumerable<ActorDto>>().Result;

            return View(actors);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int id)
        {
            DetailsActor ViewModel = new DetailsActor(); 

            //Objective: To communicate with the Actor data API to retrieve one actor
            //curl https://localhost:44343/api/actordata/findactor/{id}

            string url = "actordata/findactor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ActorDto SelectedActor = response.Content.ReadAsAsync<ActorDto>().Result;

            ViewModel.SelectedActor = SelectedActor;

            //Show all movies in which this actor has starred
            url = "moviedata/listmoviesforactor/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<MovieDto> MoviesForActor = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.MoviesForActor = MoviesForActor;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Actor/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Actor/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Actor actor)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective : Add a new actor into our system using the API
            //curl -H "Content-type:application/json" -d @actor.json https://localhost:44343/api/actordata/addactor

            string jsonpayload = jss.Serialize(actor);

            string url = "actordata/addactor";

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

        // GET: Actor/Update/5
        [Authorize]
        public ActionResult Update(int id)
        {
            //Find the actor to show to the user to understand what is being edited

            string url = "actordata/findactor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ActorDto selectedactor = response.Content.ReadAsAsync<ActorDto>().Result;

            return View(selectedactor);
        }

        // POST: Actor/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, Actor actor)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            //Objective : Add a new actor into our system using the API
            //curl -H "Content-type:application/json" -d @actor.json https://localhost:44343/api/actordata/addactor
            string url = "actordata/updateactor/" + id;

            string jsonpayload = jss.Serialize(actor);

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

        // GET: Actor/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            string url = "actordata/findactor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ActorDto selectedactor = response.Content.ReadAsAsync<ActorDto>().Result;

            return View(selectedactor);
        }

        // POST: Actor/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, Actor actor)
        {
            // Gets the asp.net application cookie to authenticate on the webapi level
            GetApplicationCookie();

            string url = "actordata/deleteactor/" + id;

            string jsonpayload = jss.Serialize(actor);

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
