namespace Scheduler_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "ProjectTime", c => c.Time(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "ProjectTime", c => c.DateTime());
        }
    }
}
