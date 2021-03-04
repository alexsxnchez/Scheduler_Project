using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler_Project.Models.ViewModels
{
    //The View Model required to update a Project
    public class UpdateProject
    {
        //Information about the player
        public ProjectDto Project { get; set; }
        //Needed for a dropdownlist which presents the player with a choice of teams to play for
        public IEnumerable<CategoryDto> Allcategories { get; set; }
    }
}