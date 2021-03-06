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
    public class InformDataController : ApiController
    {
        private SchedulerDataContext db = new SchedulerDataContext();
        /// <summary>
        ///     Returns a list of Informs in the database
        /// </summary>
        /// <example>
        ///     GET: api/InformData/GetInforms
        /// </example>
        /// <returns>A list of Informs information (Inform Id, Inform Name, Inform Description, and Inform Date)</returns> (CHECKED)
        // GET: api/InformData/GetInforms
        [HttpGet]
        [Route("api/InformData/GetInforms")]
        [ResponseType(typeof(IEnumerable<InformDto>))]
        public IHttpActionResult GetInforms()
        {
            List<Inform> Informs = db.Informs.ToList();
            List<InformDto> InformDtos = new List<InformDto> { };
            //Choosen information to expose to the API
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

        // <summary>
        ///     Finding a Inform by it's id
        /// </summary>
        /// <example> GET: api/InformData/FindInform/1 </example>
        /// <param name="id">Inform Id</param>
        /// <returns>All the information of the Inform</returns> (CHECKED)
        // GET: api/InformData/FindInform/1
        [HttpGet]
        [Route("api/InformData/FindInform/{id}")]
        [ResponseType(typeof(InformDto))]
        public IHttpActionResult FindInform(int id)
        {
            //This will look through the database and find the Inform Id
            Inform Inform = db.Informs.Find(id);
            //if not found, return 404 status code.
            if (Inform == null)
            {
                return NotFound();
            }
            InformDto InformDto = new InformDto
            {
                InformID = Inform.InformID,
                InfoData = Inform.InfoData,
                InfoPhoneNumber = Inform.InfoPhoneNumber,
                InfoEmail = Inform.InfoEmail,
                InfoUrl = Inform.InfoUrl,
                ProjectID = Inform.ProjectID
            };
            return Ok(InformDto);
        }
        /// <summary>
        ///     This Finds the Project for the Inform by the Inform Id.
        /// </summary>
        /// <example> GET: api/ProjectData/FindCetagoryForInform/1 </example>
        /// <param name="id">Inform Id</param>
        /// <returns>All the information of the Project</returns> (CHECKED)
        // GET: api/ProjectData/FindProjectForInform/1
        [HttpGet]
        [Route("api/ProjectData/FindProjectForInform/{id}")]
        [ResponseType(typeof(ProjectDto))]
        public IHttpActionResult FindProjectForInform(int id)
        {
            //Finds the first Project which has any Informs that match the inputed Inform Id.
            Project Project = db.Projects
                .Where(t => t.Informs.Any(p => p.InformID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Project == null)
            {
                return NotFound();
            }
            //put into a 'Data Transfer Object'
            ProjectDto ProjectDto = new ProjectDto
            {
                ProjectID = Project.ProjectID,
                ProjectName = Project.ProjectName,
                ProjectDate = Project.ProjectDate,
                ProjectDescription = Project.ProjectDescription
            };
            //pass along data as 200 status code OK response
            return Ok(ProjectDto);
        }
        /// <summary>
        ///     Adds a Inform to the database
        /// </summary>
        /// <example> POST: api/InformData/AddInform </example>
        /// <param name="Inform">A Inform Object</param>
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/InformData/AddInform   //A Inform[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(Inform))]
        [Route("api/InformData/AddInform")]
        [HttpPost]
        public IHttpActionResult AddInform([FromBody] Inform Inform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Add a Inform and save the changes into the database
            db.Informs.Add(Inform);
            db.SaveChanges();

            return Ok(Inform.InformID);
        }
        /// <summary>
        ///     Will Update the Inform from the database by id.
        /// </summary>
        /// <example> POST: api/InformData/UpdateInform/1 </example>
        /// <param name="id">Inform Id</param>
        /// <param name="Inform">A Inform Object</param>
        /// <returns></returns>//Updated to database (CHECKED)
        // POST: api/InformData/UpdateInform/1   //A Inform[2] object, sent as Post Form Data. Mine is not!
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/InformData/UpdateInform/{id}")]
        public IHttpActionResult UpdateInform(int id, [FromBody] Inform Inform)
        {
            //If the Model State is not valid send a Bad Request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //If the id doesn't match a Inform Id send a Bad Request
            if (id != Inform.InformID)
            {
                return BadRequest();
            }
            //Otherwise Update the inputed Inform
            db.Entry(Inform).State = EntityState.Modified;
            //Save the changes => Catch if Inform Id does not exist
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InformExists(id))
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
        ///     Finds a Inform in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Inform Id</param>
        /// <returns>TRUE if the Inform exists, false otherwise.</returns>
        private bool InformExists(int id)
        {
            return db.Informs.Count(e => e.InformID == id) > 0;
        }
        /// <summary>
        ///     Deletes a Inform form the database
        /// </summary>
        /// <example> POST: api/InformData/DeleteInform/1 </example>
        /// <param name="id">Inform Id</param>//Inform to delete by Id.
        /// <returns>Successful or Not Successful</returns> (CHECKED)
        // POST: api/InformData/DeleteInform/1
        [ResponseType(typeof(Inform))]
        [HttpPost]
        [Route("api/InformData/DeleteInform/{id}")]
        public IHttpActionResult DeleteInform(int id)
        {
            Inform Inform = db.Informs.Find(id);
            if (Inform == null)
            {
                return NotFound();
            }
            db.Informs.Remove(Inform);
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