namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genremovies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreID = c.Int(nullable: false, identity: true),
                        GenreName = c.String(),
                    })
                .PrimaryKey(t => t.GenreID);
            
            CreateTable(
                "dbo.GenreMovies",
                c => new
                    {
                        Genre_GenreID = c.Int(nullable: false),
                        Movie_MovieID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Genre_GenreID, t.Movie_MovieID })
                .ForeignKey("dbo.Genres", t => t.Genre_GenreID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieID, cascadeDelete: true)
                .Index(t => t.Genre_GenreID)
                .Index(t => t.Movie_MovieID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GenreMovies", "Movie_MovieID", "dbo.Movies");
            DropForeignKey("dbo.GenreMovies", "Genre_GenreID", "dbo.Genres");
            DropIndex("dbo.GenreMovies", new[] { "Movie_MovieID" });
            DropIndex("dbo.GenreMovies", new[] { "Genre_GenreID" });
            DropTable("dbo.GenreMovies");
            DropTable("dbo.Genres");
        }
    }
}
