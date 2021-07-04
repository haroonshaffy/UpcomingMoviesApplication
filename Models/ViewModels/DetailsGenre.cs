using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpcomingMoviesApplication.Models.ViewModels
{
    public class DetailsGenre
    {
        public GenreDto SelectedGenre { get; set; }
        public IEnumerable<MovieDto> MoviesForGenre { get; set; }
    }
}