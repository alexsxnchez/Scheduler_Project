using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Scheduler_Project.Models;
using Scheduler_Project.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Scheduler_Project.Controllers
{
    public class ProjectController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;
        public object CategoryID { get; private set; }

        static ProjectController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44356/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Project/List
        public ActionResult List()
        {
            string url = "ProjectData/GetProjects";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ProjectDto> SelectedProjects = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;
                return View(SelectedProjects);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Project/Details/1
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowProject ViewModel = new ShowProject();
            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Project Data Transfer Object
                ProjectDto SelcetedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
                ViewModel.Project = SelcetedProject;

                //Find the Category for Project by Id
                url = "CategoryData/FindCategoryForProject/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Category = SelectedCategory;

                //Get the Inform for Project by Id
                url = "ProjectData/GetInformForProject/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                IEnumerable<InformDto> SelectedInforms = response.Content.ReadAsAsync<IEnumerable<InformDto>>().Result;
                ViewModel.ProjectInforms = SelectedInforms;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            UpdateProject ViewModel = new UpdateProject();
            //get information about Categories this Project is in.
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> PotetnialCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.Allcategories = PotetnialCategories;
            return View(ViewModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectInfo"></param>
        /// <returns></returns>
        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Project ProjectInfo)
        {
            Debug.WriteLine(ProjectInfo.ProjectName);
            string url = "ProjectData/AddProject";
            //Debug.WriteLine(jss.Serialize(ProjectInfo));
            HttpContent content = new StringContent(jss.Serialize(ProjectInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                //int ProjectID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", "Category", new { id = CategoryID });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Project/Edit/2
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UpdateProject ViewModel = new UpdateProject();

            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
                ViewModel.Project = SelectedProject;

                url = "CategoryData/GetCategories";
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> ProjectsCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.Allcategories = ProjectsCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ProjectInfo"></param>
        /// <returns></returns>
        // POST: Project/Edit/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Project ProjectInfo)
        {
            Debug.WriteLine(ProjectInfo.ProjectName);
            string url = "ProjectData/UpdateProject/" + id;
            Debug.WriteLine(jss.Serialize(ProjectInfo));
            HttpContent content = new StringContent(jss.Serialize(ProjectInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Project/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
                return View(SelectedProject);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "ProjectData/DeleteProject/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Category");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
