using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Scheduler_Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class SchedulerDataContext : IdentityDbContext<ApplicationUser>
    {
        public SchedulerDataContext() : base("name=SchedulerDataContext", throwIfV1Schema: false)
        {

        }
        public static SchedulerDataContext Create()
        {
            return new SchedulerDataContext();
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