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
    public class InformController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static InformController()
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
        // GET: Inform/List
        public ActionResult List()
        {
            string url = "InformData/GetInforms";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<InformDto> SelectedInforms = response.Content.ReadAsAsync<IEnumerable<InformDto>>().Result;
                return View(SelectedInforms);
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
        // GET: Inform/Details/1
        public ActionResult Details(int id)
        {
            ShowInform ViewModel = new ShowInform();
            string url = "InformData/FindInform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Inform Data Transfer Object
                InformDto SelectedInform = response.Content.ReadAsAsync<InformDto>().Result;
                ViewModel.Inform = SelectedInform;

                //Find the Project for Inform by Id
                url = "ProjectData/FindProjectForInform/" + id;
                response = client.GetAsync(url).Result;
                Debug.WriteLine(response.StatusCode);
                ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
                ViewModel.Project = SelectedProject;

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
        // GET: Project/Create
        [HttpGet]
        public ActionResult Create()
        {
            UpdateInform ViewModel = new UpdateInform();
            //get information about Projects this Inform is in.
            string url = "ProjectData/GetProjects";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ProjectDto> PotetnialProjects = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;
            ViewModel.Allprojects = PotetnialProjects;
            return View(ViewModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InformInfo"></param>
        /// <returns></returns>
        // POST: Inform/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Inform InformInfo)
        {
            Debug.WriteLine(InformInfo.InfoData);
            string url = "InformData/AddInform";
            //Debug.WriteLine(jss.Serialize(InformInfo));
            HttpContent content = new StringContent(jss.Serialize(InformInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                int InformID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("List", "Category");
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
        // GET: Inform/Edit/2
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UpdateInform ViewModel = new UpdateInform();

            string url = "InformData/FindInform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                InformDto SelectedInform = response.Content.ReadAsAsync<InformDto>().Result;
                ViewModel.Inform = SelectedInform;

                url = "ProjectData/GetProjects";
                response = client.GetAsync(url).Result;
                IEnumerable<ProjectDto> InformsProject = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;
                ViewModel.Allprojects = InformsProject;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Inform/Edit/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Inform InformInfo)
        {
            Debug.WriteLine(InformInfo.InfoData);
            string url = "InformData/UpdateInform/" + id;
            Debug.WriteLine(jss.Serialize(InformInfo));
            HttpContent content = new StringContent(jss.Serialize(InformInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Category");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Inform/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "InformData/FindInform/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                InformDto SelectedInform = response.Content.ReadAsAsync<InformDto>().Result;
                return View(SelectedInform);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Inform/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "InformData/DeleteInform/" + id;
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