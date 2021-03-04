using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler_Project.Models.ViewModels
{
    public class ShowCategory
    {
        //Information about the team
        public CategoryDto Category { get; set; }

        //Information about all Projects on that Categories
        public IEnumerable<ProjectDto> CategoryProjects { get; set; }
    }
}