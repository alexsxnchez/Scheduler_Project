﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler_Project.Models.ViewModels
{
    public class ShowCategory
    {
        //The View needs to conditionally render the page based on admin or non admin.
        //Admin will see "Create New" and "Edit" links, non-admin will not see these.
        public bool isadmin { get; set; }
        //Information about the team
        public CategoryDto Category { get; set; }

        //Information about all Projects on that Categories
        public IEnumerable<ProjectDto> CategoryProjects { get; set; }
    }
}