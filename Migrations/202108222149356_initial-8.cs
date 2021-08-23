namespace Scheduler_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "ProjectTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "ProjectTime", c => c.DateTime(nullable: false));
        }
    }
}
