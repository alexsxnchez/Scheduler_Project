using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace Scheduler_Project.Models
{
    public class SchedulerDataContext : DbContext
    {
        public SchedulerDataContext() : base("name=SchedulerDataContext")
        {

        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }

        /*Tools -> NuGet Package Manager -> Package Manager Console
         * enable-migrations (only once)
         * add-migration {migration name}
         * update-database*/

        /*To View the Database Changes sequentially, go to Project/Migrations folder*/

        /*To View Database itself, go to View > SQL Server Object Explorer
         * (localdb)\MSSQLLocalDB
         * Right Click {ProjectName}.Models.DataContext > Tables
         * Can Right Click Tables to View Data
         * Can Right Click Database to Query*/
    }
}