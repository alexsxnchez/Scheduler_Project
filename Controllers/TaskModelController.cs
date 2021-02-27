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
    public class TaskModelController : ApiController
    {
        private SchedulerDataContext db = new SchedulerDataContext();
        /// <summary>
        ///     Returns a list of Tasks in the database
        /// </summary>
        /// <example>
        ///     GET: api/TaskData/GetTasks
        /// </example>
        /// <returns>A list of Tasks information (Task Id, Task Name, Task Description, and Task Date)</returns> (CHECKED)
        // GET: api/TaskData/GetTasks
        [HttpGet]
        [Route("api/TaskData/GetTasks")]
        [ResponseType(typeof(IEnumerable<TaskDto>))]
        public IHttpActionResult GetTasks()
        {
            List<Task> Tasks = db.Tasks.ToList();
            List<TaskDto> TaskDtos = new List<TaskDto> { };
            //Choosen information to expose to the API
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
        // <summary>
        ///     Finding a Task by it's id
        /// </summary>
        /// <example> GET: api/TaskData/FindTask/1 </example>
        /// <param name="id">Task Id</param>
        /// <returns>All the information of the Task</returns> (CHECKED)
        // GET: api/TaskData/FindTask/1
        [HttpGet]
        [Route("api/TaskData/FindTask/{id}")]
        [ResponseType(typeof(TaskDto))]
        public IHttpActionResult FindTask(int id)
        {
            //This will look through the database and find the Task Id
            Task Task = db.Tasks.Find(id);
            //if not found, return 404 status code.
            if (Task == null)
            {
                return NotFound();
            }
            TaskDto TaskDto = new TaskDto
            {
                TaskID = Task.TaskID,
                TaskName = Task.TaskName,
                TaskDescription = Task.TaskDescription,
                TaskDate = Task.TaskDate,
                CategoryID = Task.CategoryID
            };
            return Ok(TaskDto);
        }
        /// <summary>
        ///     This Finds the Category for the Task by the Task Id.
        /// </summary>
        /// <example> GET: api/CategoryData/FindCetagoryForTask/1 </example>
        /// <param name="id">Task Id</param>
        /// <returns>All the information of the Category</returns> (CHECKED)
        // GET: api/CategoryData/FindCategoryForTask/1
        [HttpGet]
        [Route("api/CategoryData/FindCategoryForTask/{id}")]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult FindCategoryForTask(int id)
        {
            //Finds the first Category which has any Tasks that match the inputed Task Id.
            Category Category = db.Categories
                .Where(t => t.Tasks.Any(p => p.TaskID == id))
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
                CategoryName = Category.CategoryName
            };
            //pass along data as 200 status code OK response
            return Ok(CategoryDto);
        }
        /// <summary>
        ///     Adds a Task to the database
        /// </summary>
        /// <example> POST: api/TaskData/AddTask </example>
        /// <param name="task">A Task Object</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/TaskData/AddTask   //A Task[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(Task))]
        [Route("api/TaskData/AddTask")]
        [HttpPost]
        public IHttpActionResult AddTask([FromBody] Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Add a Task and save the changes into the database
            db.Tasks.Add(task);
            db.SaveChanges();

            return Ok(task.TaskID);
        }
        /// <summary>
        ///     Will Update the Task from the database by id.
        /// </summary>
        /// <example> POST: api/TaskData/UpdateTask/1 </example>
        /// <param name="id">Task Id</param>
        /// <param name="task">A Task Object</param>
        /// <returns></returns>//Updated to database (CHECKED)
        // POST: api/TaskData/UpdateTask/1   //A Task[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/TaskData/UpdateTask/{id}")]
        public IHttpActionResult UpdateTask(int id, [FromBody] Task task)
        {
            //If the Model State is not valid send a Bad Request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //If the id doesn't match a Task Id send a Bad Request
            if (id != task.TaskID)
            {
                return BadRequest();
            }
            //Otherwise Update the inputed Task
            db.Entry(task).State = EntityState.Modified;
            //Save the changes => Catch if Task Id does not exist
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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
        ///     Finds a Task in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Task Id</param>
        /// <returns>TRUE if the Task exists, false otherwise.</returns>
        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.TaskID == id) > 0;
        }
        /// <summary>
        ///     Deletes a Task form the database
        /// </summary>
        /// <example> POST: api/TaskData/DeleteTask/1 </example>
        /// <param name="id">Task Id</param>//Task to delete by Id.
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/TaskData/DeleteTask/1
        [ResponseType(typeof(Task))]
        [HttpPost]
        [Route("api/TaskData/DeleteTask/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            db.Tasks.Remove(task);
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