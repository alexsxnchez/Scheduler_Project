using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler_Project.Models.ViewModels
{
    public class ShowTask
    {
        public TaskDto Task { get; set; }
        //information about the team the player plays for
        public CategoryDto Category { get; set; }
    }
}