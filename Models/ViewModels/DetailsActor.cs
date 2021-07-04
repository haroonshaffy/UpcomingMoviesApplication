using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpcomingMoviesApplication.Models.ViewModels
{
    public class DetailsActor
    {
        public ActorDto SelectedActor { get; set; }
        public IEnumerable<MovieDto> MoviesForActor { get; set; }
    }
}