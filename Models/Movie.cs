using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace UpcomingMoviesApplication.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public DateTime MovieReleaseDate { get; set; }
        
        //Movie Duration in minutes
        public int MovieDuration { get; set; }

        //TODO : Add movie poster image and youtube link to the trailer

        //A movie can belong to multiple genres
        public ICollection<Genre> Genres { get; set; }

        // A movie can have multiple actors
        public ICollection<Actor> Actors { get; set; }
    }

    public class MovieDto
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public DateTime MovieReleaseDate { get; set; }

        //Movie Duration in minutes
        public int MovieDuration { get; set; }
    }
}