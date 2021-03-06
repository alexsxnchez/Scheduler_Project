namespace Scheduler_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Informs",
                c => new
                    {
                        InformID = c.Int(nullable: false, identity: true),
                        InfoData = c.String(),
                        InfoPhoneNumber = c.String(),
                        InfoEmail = c.String(),
                        InfoUrl = c.String(),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InformID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Informs", "ProjectID", "dbo.Projects");
            DropIndex("dbo.Informs", new[] { "ProjectID" });
            DropTable("dbo.Informs");
        }
    }
}
