using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpcomingMoviesApplication.Models.ViewModels
{
    public class DetailsMovie
    {
        public MovieDto SelectedMovie { get; set; }
        public IEnumerable<ActorDto> ActorsForMovie { get; set; }

        public IEnumerable<GenreDto> GenresForMovie { get; set; }

        public IEnumerable<ActorDto> ActorsNotInMovie { get; set; }

        public IEnumerable<GenreDto> GenresNotForMovie { get; set; }
    }
}