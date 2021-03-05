using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scheduler_Project.Models;
using Scheduler_Project.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace Scheduler_Project.Controllers
{
    public class CategoryController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CategoryController()
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
        /// <returns></returns> (CHECKED)
        // GET: Category/List
        public ActionResult List()
        {
            string url = "CategoryData/GetCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CategoryDto> SelectedCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
                return View(SelectedCategories);
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
        /// <returns></returns> (CHECKED)
        // GET: Category/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            ShowCategory ViewModel = new ShowCategory();
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Category Data Transfer Object
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                ViewModel.Category = SelectedCategory;//Seen in the ViewModel Folder

                url = "CategoryData/GetProjectForCategory/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                Debug.WriteLine(response.StatusCode);
                IEnumerable<ProjectDto> SelectedProjects = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;
                ViewModel.CategoryProjects = SelectedProjects;//Seen in the ViewModel Folder

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
        /// <returns></returns> (CHECKED)
        // GET: Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryInfo"></param>
        /// <returns></returns> (CHECKED)
        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Category CategoryInfo)
        {
            Debug.WriteLine(CategoryInfo.CategoryName);
            string url = "CategoryData/AddCategory";
            HttpContent content = new StringContent(jss.Serialize(CategoryInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
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
        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Categorydata/findCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Category data transfer object
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
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
        /// <param name="CategoryInfo"></param>
        /// <returns></returns>
        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Category CategoryInfo)
        {
            Debug.WriteLine(CategoryInfo.CategoryName);
            string url = "Categorydata/updateCategory/" + id;
            Debug.WriteLine(jss.Serialize(CategoryInfo));
            HttpContent content = new StringContent(jss.Serialize(CategoryInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
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
        /// <returns></returns> (CHECKED)
        // GET: Category/DeleteConfirm/1
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
                return View(SelectedCategory);
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
        /// <returns></returns> (CHECKED)
        // POST: Category/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "CategoryData/DeleteCategory/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            Debug.WriteLine(response.StatusCode);
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