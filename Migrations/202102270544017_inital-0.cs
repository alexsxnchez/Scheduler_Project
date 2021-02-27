namespace Scheduler_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inital0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
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
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Tasks", new[] { "CategoryID" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Categories");
        }
    }
}
