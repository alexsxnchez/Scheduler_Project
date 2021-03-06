using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Scheduler_Project.Models
{
    public class Inform
    {
        [Key]
        public int InformID { get; set; }
        public string InfoData { get; set; }
        public string InfoPhoneNumber { get; set; }
        public string InfoEmail { get; set; }
        public string InfoUrl { get; set; }

        [ForeignKey("Project")]
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }
    }

    //Inform Data Transfer Object (Dto) -> This is to secure the Project class.
    public class InformDto
    {
        public int InformID { get; set; }
        [DisplayName("Extra Information")]
        public string InfoData { get; set; }
        [DisplayName("Phone Number")]
        public string InfoPhoneNumber { get; set; }
        [DisplayName("Email")]
        public string InfoEmail { get; set; }
        [DisplayName("Links")]
        public string InfoUrl { get; set; }

        public int ProjectID { get; set; }
    }
}