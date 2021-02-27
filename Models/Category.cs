using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Scheduler_Project.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }

    //Task Data Transfer Object (Dto) -> This is to secure the Task class.
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}