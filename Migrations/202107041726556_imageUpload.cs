namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "AnimalHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movies", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "PicExtension");
            DropColumn("dbo.Movies", "AnimalHasPic");
        }
    }
}
