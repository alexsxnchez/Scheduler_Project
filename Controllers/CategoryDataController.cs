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
        ///     Returns a kist of Task in the database
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
        /// <example> GET: api/CategoryData/GetTaskForCategory </example>
        /// <param name="id">The Category Id</param>
        /// <returns>Returns a list of Tasks associated with the Category</returns> (CHECKED)
        // GET: api/CategoryData/GetTaskForCategory
        [HttpGet]
        [Route("api/CategoryData/GetTaskForCategory/{id}")]
        [ResponseType(typeof(IEnumerable<TaskDto>))]
        public IHttpActionResult GetTaskForCategory(int id)
        {
            List<Task> Tasks = db.Tasks.Where(t => t.CategoryID == id)
                .ToList();
            List<TaskDto> TaskDtos = new List<TaskDto> { };

            foreach (var Task in Tasks)
            {
                TaskDto NewTask = new TaskDto
                {
                    TaskID = Task.TaskID,
                    TaskName = Task.TaskName,
                    TaskDescription = Task.TaskDescription,
                    TaskDate = Task.TaskDate,
                    CategoryID = Task.CategoryID
                };
                TaskDtos.Add(NewTask);
            }
            return Ok(TaskDtos);
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