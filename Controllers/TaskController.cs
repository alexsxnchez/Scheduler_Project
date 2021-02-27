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
    public class TaskController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static TaskController()
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
        // GET: Task/List
        public ActionResult List()
        {
            string url = "TaskData/GetTasks";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<TaskDto> SelectedTasks = response.Content.ReadAsAsync<IEnumerable<TaskDto>>().Result;
                return View(SelectedTasks);
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
        // GET: Task/Details/1
        public ActionResult Details(int id)
        {
            ShowTask ViewModel = new ShowTask();
            string url = "TaskData/FindTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Task Data Transfer Object
                TaskDto SelcetedTask = response.Content.ReadAsAsync<TaskDto>().Result;
                ViewModel.Task = SelcetedTask;
                //Find the Category for Task by Id
                url = "CategoryData/FindCategoryForTask/" + id;
                response = client.GetAsync(url).Result;
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Category = SelectedCategory;

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
            UpdateTask ViewModel = new UpdateTask();
            //get information about Categories this Task is in.
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> PotetnialCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            ViewModel.Allcategories = PotetnialCategories;
            return View(ViewModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TaskInfo"></param>
        /// <returns></returns>
        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Task TaskInfo)
        {
            Debug.WriteLine(TaskInfo.TaskName);
            string url = "TaskData/AddTask";
            //Debug.WriteLine(jss.Serialize(TaskInfo));
            HttpContent content = new StringContent(jss.Serialize(TaskInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                int TaskID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = TaskID });
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
        // GET: Task/Edit/2
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UpdateTask ViewModel = new UpdateTask();

            string url = "TaskData/FindTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                TaskDto SelectedTask = response.Content.ReadAsAsync<TaskDto>().Result;
                ViewModel.Task = SelectedTask;

                url = "CategoryData/GetCategories";
                response = client.GetAsync(url).Result;
                IEnumerable<CategoryDto> TasksCategory = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                ViewModel.Allcategories = TasksCategory;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Task/Edit/2
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Task TaskInfo)
        {
            Debug.WriteLine(TaskInfo.TaskName);
            string url = "TaskData/UpdateTask/" + id;
            Debug.WriteLine(jss.Serialize(TaskInfo));
            HttpContent content = new StringContent(jss.Serialize(TaskInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Task/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TaskData/FindTask/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                TaskDto SelectedTask = response.Content.ReadAsAsync<TaskDto>().Result;
                return View(SelectedTask);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Task/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "TaskData/DeleteTask/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
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
