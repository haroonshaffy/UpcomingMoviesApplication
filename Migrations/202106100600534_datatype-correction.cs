namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatypecorrection : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Actors", "ActorGender", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Actors", "ActorGender", c => c.Int(nullable: false));
        }
    }
}
