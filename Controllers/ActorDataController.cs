using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UpcomingMoviesApplication.Models;

namespace UpcomingMoviesApplication.Controllers
{
    public class ActorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ActorData/ListActors
        [HttpGet]
        public IEnumerable<ActorDto> ListActors()
        {
            List<Actor> Actors = db.Actors.ToList();
            List<ActorDto> ActorDtos = new List<ActorDto>();

            Actors.ForEach(b => ActorDtos.Add(new ActorDto()
            {
                ActorID = b.ActorID,
                ActorName = b.ActorName,
                ActorAge = b.ActorAge,
                ActorGender = b.ActorGender
            }));

            return ActorDtos;
        }

        /// <summary>
        /// Returns all Actors in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All Actors in the database, for a particular movie
        /// </returns>
        /// <param name="id">Movie primary key</param>
        /// <example>
        /// GET: api/ActorData/ListActorsForMovie/5
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(ActorDto))]
        public IHttpActionResult ListActorsForMovie(int id)
        {
            List<Actor> Actors = db.Actors.Where(b => b.Movies.Any(a => a.MovieID == id)).ToList();
            List<ActorDto> ActorDtos = new List<ActorDto>();

            Actors.ForEach(b => ActorDtos.Add(new ActorDto()
            {
                ActorID = b.ActorID,
                ActorName = b.ActorName,
                ActorAge = b.ActorAge,
                ActorGender = b.ActorGender
            }));
            return Ok(ActorDtos);
        }

        /// <summary>
        /// Returns all Actors in the system
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All Actors in the database not in that particular movie
        /// </returns>
        /// <param name="id">Movie primary key</param>
        /// <example>
        /// GET: api/ActorData/ListActorsNotInMovie/5
        /// </example>

        [HttpGet]
        [ResponseType(typeof(ActorDto))]
        public IHttpActionResult ListActorsNotInMovie(int id)
        {
            List<Actor> Actors = db.Actors.Where(b => !b.Movies.Any(a => a.MovieID == id)).ToList();
            List<ActorDto> ActorDtos = new List<ActorDto>();

            Actors.ForEach(b => ActorDtos.Add(new ActorDto()
            {
                ActorID = b.ActorID,
                ActorName = b.ActorName,
                ActorAge = b.ActorAge,
                ActorGender = b.ActorGender
            }));
            return Ok(ActorDtos);
        }



        // GET: api/ActorData/FindActor/5
        [ResponseType(typeof(Actor))]
        [HttpGet]
        public IHttpActionResult FindActor(int id)
        {
            Actor actor = db.Actors.Find(id);
            ActorDto ActorDto = new ActorDto()
            {
                ActorID = actor.ActorID,
                ActorName = actor.ActorName,
                ActorAge = actor.ActorAge,
                ActorGender = actor.ActorGender
            };

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(ActorDto);
        }

        // POST: api/ActorData/UpdateActor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateActor(int id, Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actor.ActorID)
            {
                return BadRequest();
            }

            db.Entry(actor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ActorData/AddActor
        [ResponseType(typeof(Actor))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddActor(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Actors.Add(actor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = actor.ActorID }, actor);
        }

        // POST: api/ActorData/DeleteActor/5
        [ResponseType(typeof(Actor))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteActor(int id)
        {
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return NotFound();
            }

            db.Actors.Remove(actor);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActorExists(int id)
        {
            return db.Actors.Count(e => e.ActorID == id) > 0;
        }
    }
}