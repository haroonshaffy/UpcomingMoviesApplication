namespace UpcomingMoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actors",
                c => new
                    {
                        ActorID = c.Int(nullable: false, identity: true),
                        ActorName = c.String(),
                        ActorAge = c.Int(nullable: false),
                        ActorGender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Actors");
        }
    }
}
