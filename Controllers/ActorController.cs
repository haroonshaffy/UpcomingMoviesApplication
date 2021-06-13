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
    public class ActorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ActorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44343/api/actordata/");
        }


        // GET: Actor/List
        public ActionResult List()
        {
            //Objective: To communicate with the Actor data API to retrieve the list of Actors
            //curl https://localhost:44343/api/actordata/listactors

            string url = "listactors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<Actor> actor = response.Content.ReadAsAsync<IEnumerable<Actor>>().Result;

            return View(actor);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int id)
        {
            //Objective: To communicate with the Actor data API to retrieve one actor
            //curl https://localhost:44343/api/actordata/findactor/{id}

            string url = "findactor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Actor selectedactor = response.Content.ReadAsAsync<Actor>().Result;

            return View(selectedactor);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Actor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Actor/Create
        [HttpPost]
        public ActionResult Create(Actor actor)
        {
            //Objective : Add a new actor into our system using the API
            //curl -H "Content-type:application/json" -d @actor.json https://localhost:44343/api/actordata/addactor

            string jsonpayload = jss.Serialize(actor);

            string url = "addactor";

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
        public ActionResult Update(int id)
        {
            //Find the actor to show to the user to understand what is being edited

            string url = "findactor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Actor selectedactor = response.Content.ReadAsAsync<Actor>().Result;

            return View(selectedactor);
        }

        // POST: Actor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Actor actor)
        {
            //Objective : Add a new actor into our system using the API
            //curl -H "Content-type:application/json" -d @actor.json https://localhost:44343/api/actordata/addactor
            string url = "updateactor/" + id;

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
        public ActionResult Delete(int id)
        {
            string url = "findactor/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            Actor selectedactor = response.Content.ReadAsAsync<Actor>().Result;

            return View(selectedactor);
        }

        // POST: Actor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Actor actor)
        {
            string url = "deleteactor/" + id;

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
