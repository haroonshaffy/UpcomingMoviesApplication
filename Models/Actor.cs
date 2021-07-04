using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UpcomingMoviesApplication.Models
{
    public class Actor
    {
        [Key]
        public int ActorID { get; set; }
        public string ActorName { get; set; }
        public int ActorAge { get; set; }
        public string ActorGender { get; set; }

        // An actor can star in multiple movies
        public ICollection<Movie> Movies { get; set; }
    }

    public class ActorDto
    {
        public int ActorID { get; set; }
        public string ActorName { get; set; }
        public int ActorAge { get; set; }
        public string ActorGender { get; set; }
    }
}