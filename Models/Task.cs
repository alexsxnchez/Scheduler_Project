using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Scheduler_Project.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
    
    //Task Data Transfer Object (Dto) -> This is to secure the Task class.
    public class TaskDto
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDate { get; set; }
        public int CategoryID { get; set; }
    }
}