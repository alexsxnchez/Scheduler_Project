namespace Scheduler_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inital1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Tasks", new[] { "CategoryID" });
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        ProjectDescription = c.String(),
                        ProjectDate = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            DropTable("dbo.Tasks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                        TaskDate = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID);
            
            DropForeignKey("dbo.Projects", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Projects", new[] { "CategoryID" });
            DropTable("dbo.Projects");
            CreateIndex("dbo.Tasks", "CategoryID");
            AddForeignKey("dbo.Tasks", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
    }
}
