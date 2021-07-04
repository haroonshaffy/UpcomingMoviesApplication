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
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MovieData/ListMovies
        [HttpGet]
        public IEnumerable<MovieDto> ListMovies()
        {
            List<Movie> Movies = db.Movies.ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieDescription = a.MovieDescription,
                MovieReleaseDate = a.MovieReleaseDate,
                MovieDuration = a.MovieDuration
            }));

            return MovieDtos;
        }

        /// <summary>
        /// Gathers information about movies related to a particular actor
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All movies in the database, including the associated actors
        /// </returns>
        /// <param name="id">Actor ID</param>
        /// <example>
        /// GET: api/MovieData/ListMoviesForActor/3
        /// </example>
        
        [HttpGet]
        [ResponseType(typeof(MovieDto))]

        public IHttpActionResult ListMoviesForActor(int id)
        {
            //All movies that have actors which match with our ID
            List<Movie> Movies = db.Movies.Where(a => a.Actors.Any(b => b.ActorID == id)).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieDescription = a.MovieDescription,
                MovieReleaseDate = a.MovieReleaseDate,
                MovieDuration = a.MovieDuration
            }));

            return Ok(MovieDtos);
        }

        /// <summary>
        /// Associates a particular Actor with a particular movie
        /// </summary>
        /// <param name="movieid"> The movieID primary key</param>
        /// <param name="actorid"> The actorID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/AssociateMovieWithActor/1/24
        /// </example>
        [HttpPost]
        [Route("api/MovieData/AssociateMovieWithActor/{movieid}/{actorid}")]
        public IHttpActionResult AssociateMovieWithActor(int movieid, int actorid)
        {
            Movie SelectedMovie = db.Movies.Include(a => a.Actors).Where(a => a.MovieID == movieid).FirstOrDefault();
            Actor SelectedActor = db.Actors.Find(actorid);

            if(SelectedMovie == null || SelectedActor == null)
            {
                return NotFound();
            }

            SelectedMovie.Actors.Add(SelectedActor);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an association between a particular Actor and a particular movie
        /// </summary>
        /// <param name="movieid"> The movieID primary key</param>
        /// <param name="actorid"> The actorID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/UnAssociateMovieWithActor/1/24
        /// </example>
        [HttpPost]
        [Route("api/MovieData/UnAssociateMovieWithActor/{movieid}/{actorid}")]
        public IHttpActionResult UnAssociateMovieWithActor(int movieid, int actorid)
        {
            Movie SelectedMovie = db.Movies.Include(a => a.Actors).Where(a => a.MovieID == movieid).FirstOrDefault();
            Actor SelectedActor = db.Actors.Find(actorid);

            if (SelectedMovie == null || SelectedActor == null)
            {
                return NotFound();
            }

            SelectedMovie.Actors.Remove(SelectedActor);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gathers information about movies related to a particular genre
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All movies in the database, including the associated actors
        /// </returns>
        /// <param name="id">Genre ID</param>
        /// <example>
        /// GET: api/MovieData/ListMoviesForGenre/3
        /// </example>

        [HttpGet]
        [ResponseType(typeof(MovieDto))]

        public IHttpActionResult ListMoviesForGenre(int id)
        {
            //All movies that have genres which match with our ID
            List<Movie> Movies = db.Movies.Where(a => a.Genres.Any(c => c.GenreID == id)).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieDescription = a.MovieDescription,
                MovieReleaseDate = a.MovieReleaseDate,
                MovieDuration = a.MovieDuration
            }));

            return Ok(MovieDtos);
        }

        /// <summary>
        /// Associates a particular Genre with a particular movie
        /// </summary>
        /// <param name="movieid"> The movieID primary key</param>
        /// <param name="genreid"> The genreID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/AssociateMovieWithGenre/1/11
        /// </example>
        [HttpPost]
        [Route("api/MovieData/AssociateMovieWithGenre/{movieid}/{genreid}")]
        public IHttpActionResult AssociateMovieWithGenre(int movieid, int genreid)
        {
            Movie SelectedMovie = db.Movies.Include(a => a.Genres).Where(a => a.MovieID == movieid).FirstOrDefault();
            Genre SelectedGenre = db.Genres.Find(genreid);

            if (SelectedMovie == null || SelectedGenre == null)
            {
                return NotFound();
            }

            SelectedMovie.Genres.Add(SelectedGenre);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Unassociates a particular Genre from a particular movie
        /// </summary>
        /// <param name="movieid"> The movieID primary key</param>
        /// <param name="genreid"> The genreID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/MovieData/UnAssociateMovieWithGenre/1/11
        /// </example>
        [HttpPost]
        [Route("api/MovieData/UnAssociateMovieWithGenre/{movieid}/{genreid}")]
        public IHttpActionResult UnAssociateMovieWithGenre(int movieid, int genreid)
        {
            Movie SelectedMovie = db.Movies.Include(a => a.Genres).Where(a => a.MovieID == movieid).FirstOrDefault();
            Genre SelectedGenre = db.Genres.Find(genreid);

            if (SelectedMovie == null || SelectedGenre == null)
            {
                return NotFound();
            }

            SelectedMovie.Genres.Remove(SelectedGenre);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/MovieData/FindMovie/5
        [ResponseType(typeof(Movie))]
        [HttpGet]
        public IHttpActionResult FindMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            MovieDto MovieDto = new MovieDto()
            {
                MovieID = movie.MovieID,
                MovieTitle = movie.MovieTitle,
                MovieDescription = movie.MovieDescription,
                MovieReleaseDate = movie.MovieReleaseDate,
                MovieDuration = movie.MovieDuration
            };

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(MovieDto);
        }

        // POST: api/MovieData/UpdateMovie/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.MovieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/MovieData/AddMovie
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult AddMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.MovieID }, movie);
        }

        // POST: api/MovieData/DeleteMovie/5
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
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

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.MovieID == id) > 0;
        }
    }
}