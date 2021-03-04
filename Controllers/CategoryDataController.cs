using System;
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
    public class CategoryDataController : ApiController
    {
        private SchedulerDataContext db = new SchedulerDataContext();
        /// <summary>
        ///     Returns a List of Categories in the database
        /// </summary>
        /// <example> GET: api/CategoryData/GetCategories </example>
        /// <returns>A list of Category information (Categroy Id, Category Name)</returns> (CHECKED)
        // GET: api/CategoryData/GetCategories
        [HttpGet]
        [Route("api/CategoryData/GetCategories")]
        [ResponseType(typeof(IEnumerable<CategoryDto>))]
        public IHttpActionResult GetCategories()
        {
            List<Category> Categories = db.Categories.ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto> { };

            //Choose which information is exposed to the API
            foreach (var Category in Categories)
            {
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryID = Category.CategoryID,
                    CategoryName = Category.CategoryName
                };
                //Add the Category name to the list
                CategoryDtos.Add(NewCategory);
            }
            return Ok(CategoryDtos);
        }
        /// <summary>
        ///     Finding a Category by it's id
        /// </summary>
        /// <example> GET: api/CategoryData/FindCategory/1 </example>
        /// <param name="id">Caategory Id</param>
        /// <returns>All the information of the Category</returns> (CHECKED)
        // GET: api/CategoryData/FindCategory/1
        [HttpGet]
        [Route("api/CategoryData/FindCategory/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }
            CategoryDto CategoryDto = new CategoryDto
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName
            };
            return Ok(CategoryDto);
        }
        /// <summary>
        ///      Gets a list of Categories in the database
        /// </summary>
        /// <example> GET: api/CategoryData/GetProjectForCategory </example>
        /// <param name="id">The Category Id</param>
        /// <returns>Returns a list of Projects associated with the Category</returns> (CHECKED)
        // GET: api/CategoryData/GetProjectForCategory
        [HttpGet]
        [Route("api/CategoryData/GetProjectForCategory/{id}")]
        [ResponseType(typeof(IEnumerable<ProjectDto>))]
        public IHttpActionResult GetProjectForCategory(int id)
        {
            List<Project> Projects = db.Projects.Where(t => t.CategoryID == id)
                .ToList();
            List<ProjectDto> ProjectDtos = new List<ProjectDto> { };

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
        /// <summary>
        ///     Adds a Category to the database.
        /// </summary>
        /// <example> POST: api/CategoryData/AddCategory </example>
        /// <param name="Category">A Category object</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/CategoryData/AddCategory
        [HttpPost]
        [Route("api/CategoryData/AddCategory")]
        [ResponseType(typeof(Category))]
        public IHttpActionResult AddCategory([FromBody] Category Category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Categories.Add(Category);
            db.SaveChanges();

            return Ok(Category.CategoryID);
        }
        /// <summary>
        ///     Deletes a Category in the database
        /// </summary>
        /// <example> POST: api/CategoryData/DeleteCategory/4 </example>
        /// <param name="id">The id of the Category to delete</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/CategoryData/DeleteCategory/4
        [HttpPost]
        [Route("api/CategoryData/DeleteCategory/{id}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            if (Category == null)
            {
                return NotFound();
            }
            db.Categories.Remove(Category);
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