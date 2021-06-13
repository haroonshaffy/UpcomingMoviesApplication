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
    public class GenreDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GenreData/ListGenre
        [HttpGet]
        public IQueryable<Genre> ListGenre()
        {
            return db.Genre;
        }

        // GET: api/GenreData/FindGenre/5
        [ResponseType(typeof(Genre))]
        [HttpGet]
        public IHttpActionResult FindGenre(int id)
        {
            Genre genre = db.Genre.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        // POST: api/GenreData/UpdateGenre/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGenre(int id, Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genre.GenreID)
            {
                return BadRequest();
            }

            db.Entry(genre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/GenreData/AddGenre
        [ResponseType(typeof(Genre))]
        [HttpPost]
        public IHttpActionResult AddGenre(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Genre.Add(genre);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = genre.GenreID }, genre);
        }

        // POST: api/GenreData/DeleteGenre/5
        [ResponseType(typeof(Genre))]
        [HttpPost]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genre.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            db.Genre.Remove(genre);
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

        private bool GenreExists(int id)
        {
            return db.Genre.Count(e => e.GenreID == id) > 0;
        }
    }
}