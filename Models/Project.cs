using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Scheduler_Project.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm AM/PM}")]
        public DateTime? ProjectTime { get; set; }
        public string ProjectDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        //Added Extra Information Table
        public ICollection<Inform> Informs { get; set; }
    }

    //Project Data Transfer Object (Dto) -> This is to secure the Project class.
    public class ProjectDto
    {
        public int ProjectID { get; set; }
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        [DisplayName("Description")]
        public string ProjectDescription { get; set; }
        [DisplayName("Project Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime? ProjectTime { get; set; }
        [DisplayName("Project Date")]
        public string ProjectDate { get; set; }
        [DisplayName("Category Id")]
        public int CategoryID { get; set; }
    }
}