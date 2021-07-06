namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageUploadcorrection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "MovieHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Movies", "AnimalHasPic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "AnimalHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Movies", "MovieHasPic");
        }
    }
}
