using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UpcomingMoviesApplication.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; }

        // A genre can have many movies
        public ICollection<Movie> Movies { get; set; }
    }

    public class GenreDto
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; }
    }
}