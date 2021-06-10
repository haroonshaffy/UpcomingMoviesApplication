namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actormovies : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MovieGenres", newName: "GenreMovies");
            DropPrimaryKey("dbo.GenreMovies");
            CreateTable(
                "dbo.MovieActors",
                c => new
                    {
                        Movie_MovieID = c.Int(nullable: false),
                        Actor_ActorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_MovieID, t.Actor_ActorID })
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .ForeignKey("dbo.Actors", t => t.Actor_ActorID, cascadeDelete: true)
                .Index(t => t.Movie_MovieID)
                .Index(t => t.Actor_ActorID);
            
            AddPrimaryKey("dbo.GenreMovies", new[] { "Genre_GenreID", "Movie_MovieID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieActors", "Actor_ActorID", "dbo.Actors");
            DropForeignKey("dbo.MovieActors", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.MovieActors", new[] { "Actor_ActorID" });
            DropIndex("dbo.MovieActors", new[] { "Movie_MovieID" });
            DropPrimaryKey("dbo.GenreMovies");
            DropTable("dbo.MovieActors");
            AddPrimaryKey("dbo.GenreMovies", new[] { "Movie_MovieID", "Genre_GenreID" });
            RenameTable(name: "dbo.GenreMovies", newName: "MovieGenres");
        }
    }
}
