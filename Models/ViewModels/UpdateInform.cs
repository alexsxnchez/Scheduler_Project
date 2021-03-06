using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scheduler_Project.Models.ViewModels
{
    //The View Model required to update a Project
    public class UpdateInform
    {
        //Information about the player
        public InformDto Inform { get; set; }
        //Needed for a dropdownlist which presents the player with a choice of teams to play for
        public IEnumerable<ProjectDto> Allprojects { get; set; }
    }
}