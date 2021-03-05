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
        public string CategoryColor { get; set; }
        public ICollection<Project> Projects { get; set; }
    }

    //CategoryData Transfer Object (Dto) -> This is to secure the Category class.
    public class CategoryDto
    {
        public int CategoryID { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [DisplayName("Color")]
        public string CategoryColor { get; set; }
    }
}