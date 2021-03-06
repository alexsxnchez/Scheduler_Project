using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Scheduler_Project.Models;
using System.Diagnostics;

namespace Scheduler_Project.Controllers
{
    public class ProjectDataController : ApiController
    {
        private SchedulerDataContext db = new SchedulerDataContext();
        /// <summary>
        ///     Returns a list of Projects in the database
        /// </summary>
        /// <example>
        ///     GET: api/ProjectData/GetProjects
        /// </example>
        /// <returns>A list of Projects information (Project Id, Project Name, Project Description, and Project Date)</returns> (CHECKED)
        // GET: api/ProjectData/GetProjects
        [HttpGet]
        [Route("api/ProjectData/GetProjects")]
        [ResponseType(typeof(IEnumerable<ProjectDto>))]
        public IHttpActionResult GetProjects()
        {
            List<Project> Projects = db.Projects.ToList();
            List<ProjectDto> ProjectDtos = new List<ProjectDto> { };
            //Choosen information to expose to the API
            foreach (var Project in Projects)
            {
                ProjectDto NewProject = new ProjectDto
                {
                    ProjectID = Project.ProjectID,
                    ProjectName = Project.ProjectName,
                    ProjectDescription = Project.ProjectDescription,
                    ProjectDate = Project.ProjectDate,
                    CategoryID = Project.CategoryID
                };
                ProjectDtos.Add(NewProject);
            }
            return Ok(ProjectDtos);
        }

        // <summary>
        ///     Finding a Project by it's id
        /// </summary>
        /// <example> GET: api/ProjectData/FindProject/1 </example>
        /// <param name="id">Project Id</param>
        /// <returns>All the information of the Project</returns> (CHECKED)
        // GET: api/ProjectData/FindProject/1
        [HttpGet]
        [Route("api/ProjectData/FindProject/{id}")]
        [ResponseType(typeof(ProjectDto))]
        public IHttpActionResult FindProject(int id)
        {
            //This will look through the database and find the Project Id
            Project Project = db.Projects.Find(id);
            //if not found, return 404 status code.
            if (Project == null)
            {
                return NotFound();
            }
            ProjectDto ProjectDto = new ProjectDto
            {
                ProjectID = Project.ProjectID,
                ProjectName = Project.ProjectName,
                ProjectDescription = Project.ProjectDescription,
                ProjectDate = Project.ProjectDate,
                CategoryID = Project.CategoryID
            };
            return Ok(ProjectDto);
        }
        /// <summary>
        ///     This Finds the Category for the Project by the Project Id.
        /// </summary>
        /// <example> GET: api/CategoryData/FindCetagoryForProject/1 </example>
        /// <param name="id">Project Id</param>
        /// <returns>All the information of the Category</returns> (CHECKED)
        // GET: api/CategoryData/FindCategoryForProject/1
        [HttpGet]
        [Route("api/CategoryData/FindCategoryForProject/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategoryForProject(int id)
        {
            //Finds the first Category which has any Projects that match the inputed Project Id.
            Category Category = db.Categories
                .Where(t => t.Projects.Any(p => p.ProjectID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Category == null)
            {
                return NotFound();
            }
            //put into a 'Data Transfer Object'
            CategoryDto CategoryDto = new CategoryDto
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName,
                CategoryColor = Category.CategoryColor
            };
            //pass along data as 200 status code OK response
            return Ok(CategoryDto);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/ProjectData/GetInformForProject/{id}")]
        [ResponseType(typeof(IEnumerable<InformDto>))]
        public IHttpActionResult GetInformForProject(int id)
        {
            List<Inform> Informs = db.Informs.Where(t => t.ProjectID == id)
                .ToList();
            List<InformDto> InformDtos = new List<InformDto> { };

            foreach (var Inform in Informs)
            {
                InformDto NewInform = new InformDto
                {
                    InformID = Inform.InformID,
                    InfoData = Inform.InfoData,
                    InfoPhoneNumber = Inform.InfoPhoneNumber,
                    InfoEmail = Inform.InfoEmail,
                    InfoUrl = Inform.InfoUrl,
                    ProjectID = Inform.ProjectID
                };
                InformDtos.Add(NewInform);
            }
            return Ok(InformDtos);
        }
        /// <summary>
        ///     Adds a Project to the database
        /// </summary>
        /// <example> POST: api/ProjectData/AddProject </example>
        /// <param name="Project">A Project Object</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/ProjectData/AddProject   //A Project[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(Project))]
        [Route("api/ProjectData/AddProject")]
        [HttpPost]
        public IHttpActionResult AddProject([FromBody] Project Project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Add a Project and save the changes into the database
            db.Projects.Add(Project);
            db.SaveChanges();

            return Ok(Project.ProjectID);
        }
        /// <summary>
        ///     Will Update the Project from the database by id.
        /// </summary>
        /// <example> POST: api/ProjectData/UpdateProject/1 </example>
        /// <param name="id">Project Id</param>
        /// <param name="Project">A Project Object</param>
        /// <returns></returns>//Updated to database (CHECKED)
        // POST: api/ProjectData/UpdateProject/1   //A Project[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/ProjectData/UpdateProject/{id}")]
        public IHttpActionResult UpdateProject(int id, [FromBody] Project Project)
        {
            //If the Model State is not valid send a Bad Request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //If the id doesn't match a Project Id send a Bad Request
            if (id != Project.ProjectID)
            {
                return BadRequest();
            }
            //Otherwise Update the inputed Project
            db.Entry(Project).State = EntityState.Modified;
            //Save the changes => Catch if Project Id does not exist
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        ///     Finds a Project in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Project Id</param>
        /// <returns>TRUE if the Project exists, false otherwise.</returns>
        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
        }
        /// <summary>
        ///     Deletes a Project form the database
        /// </summary>
        /// <example> POST: api/ProjectData/DeleteProject/1 </example>
        /// <param name="id">Project Id</param>//Project to delete by Id.
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/ProjectData/DeleteProject/1
        [ResponseType(typeof(Project))]
        [HttpPost]
        [Route("api/ProjectData/DeleteProject/{id}")]
        public IHttpActionResult DeleteProject(int id)
        {
            Project Project = db.Projects.Find(id);
            if (Project == null)
            {
                return NotFound();
            }
            db.Projects.Remove(Project);
            db.SaveChanges();

            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}