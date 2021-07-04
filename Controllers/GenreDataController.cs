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

        // GET: api/GenreData/ListGenres
        [HttpGet]
        public IEnumerable<GenreDto> ListGenres()
        {
            List<Genre> Genres = db.Genres.ToList();
            List<GenreDto> GenreDtos = new List<GenreDto>();

            Genres.ForEach(c => GenreDtos.Add(new GenreDto()
            {
                GenreID = c.GenreID,
                GenreName = c.GenreName
            }));

            return GenreDtos;
        }

        /// <summary>
        /// Gathers information about genres related to a particular movie
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All Genres in the database, associated with a particular movie
        /// </returns>
        /// <param name="id">Movie Primary Key</param>
        /// <example>
        /// GET: api/MovieData/ListGenresForMovie/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(MovieDto))]

        public IHttpActionResult ListGenresForMovie(int id)
        {
            //All movies that have genres which match with our ID
            List<Genre> Genres = db.Genres.Where(c => c.Movies.Any(a => a.MovieID == id)).ToList();
            List<GenreDto> GenreDtos = new List<GenreDto>();

            Genres.ForEach(c => GenreDtos.Add(new GenreDto()
            {
                GenreID = c.GenreID,
                GenreName = c.GenreName
            }));

            return Ok(GenreDtos);
        }

        /// <summary>
        /// Gathers information about genres not related to a particular movie
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All Genres in the database, not associated with a particular movie
        /// </returns>
        /// <param name="id">Movie Primary Key</param>
        /// <example>
        /// GET: api/GenreData/ListGenresNotForMovie/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(GenreDto))]

        public IHttpActionResult ListGenresNotForMovie(int id)
        {
            //All movies that have genres which match with our ID
            List<Genre> Genres = db.Genres.Where(c => !c.Movies.Any(a => a.MovieID == id)).ToList();
            List<GenreDto> GenreDtos = new List<GenreDto>();

            Genres.ForEach(c => GenreDtos.Add(new GenreDto()
            {
                GenreID = c.GenreID,
                GenreName = c.GenreName
            }));

            return Ok(GenreDtos);
        }

        // GET: api/GenreData/FindGenre/5
        [ResponseType(typeof(Genre))]
        [HttpGet]
        public IHttpActionResult FindGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            GenreDto GenreDto = new GenreDto()
            {
                GenreID = genre.GenreID,
                GenreName = genre.GenreName
            };

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(GenreDto);
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

            db.Genres.Add(genre);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = genre.GenreID }, genre);
        }

        // POST: api/GenreData/DeleteGenre/5
        [ResponseType(typeof(Genre))]
        [HttpPost]
        public IHttpActionResult DeleteGenre(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return NotFound();
            }

            db.Genres.Remove(genre);
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
            return db.Genres.Count(c => c.GenreID == id) > 0;
        }
    }
}