namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genre : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GenreMovies", newName: "MovieGenres");
            DropPrimaryKey("dbo.MovieGenres");
            AddPrimaryKey("dbo.MovieGenres", new[] { "Movie_MovieID", "Genre_GenreID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MovieGenres");
            AddPrimaryKey("dbo.MovieGenres", new[] { "Genre_GenreID", "Movie_MovieID" });
            RenameTable(name: "dbo.MovieGenres", newName: "GenreMovies");
        }
    }
}
