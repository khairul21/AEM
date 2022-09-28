using MvcLoginRegister.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Login = MvcLoginRegister.Models.Login;

namespace MvcLoginRegister.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
           using (OurDbContext db = new OurDbContext())
            {
                return View(db.userAccount.ToList());
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Name + "successful registered";
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (OurDbContext db = new OurDbContext())
            {
                //string uri = "http://test-demo.aemenersol.com/api/Account/Login";
                //using (HttpClient httpClient = new HttpClient())
                //{
                //    List<KeyValuePair<string, string>> requestParams = new List<KeyValuePair<string, string>>
                //    {
                //        new KeyValuePair<string, string>("Email", user.Username),
                //    new KeyValuePair<string, string>("Password", user.Password),
                //};

                //    FormUrlEncodedContent requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                //    HttpResponseMessage response = httpClient.PostAsync(uri, requestParamsFormUrlEncoded).Result;
                //    string responseStr = response.Content.ReadAsStringAsync().Result;
                    
                //}
                var login = new Login()
                {
                    username = user.Username,
                    password = user.Password,
                };

                string content = string.Empty;
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage httpResponse = null;

                //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = httpClient.PostAsJsonAsync("http://test-demo.aemenersol.com/api/Account/Login", login).Result;  //.PostAsync(url, httpContent);
                string resp = httpResponse.Content.ReadAsStringAsync().Result;
                //apiResponse getResidential = JsonConvert.DeserializeObject<apiResponse>(resp);

                //var usr = db.userAccount.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                if (resp != null)
                {
                    return RedirectToAction("LoggedIn",new {token=resp});
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong.");
                    return View();
                }
                
            }
        }
        public ActionResult LoggedIn(string token)
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult LoggedIn(UniqueDetail unique)
        {
            var url = "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGFlbWVuZXJzb2wuY29tIiwianRpIjoiYThiZDkzNTYtNmU1Mi00ZmJiLTljYTQtZmIxOTJjOGViZDFlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzMzE4ZTcxMC05MzAzLTQ4ZmQtODNjNS1mYmNhOTU0MTExZWYiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjY2OTMyODQyLCJpc3MiOiJodHRwOi8vdGVzdC1kZW1vLmFlbWVuZXJzb2wuY29tIiwiYXVkIjoiaHR0cDovL3Rlc3QtZGVtby5hZW1lbmVyc29sLmNvbSJ9.aDBf4nSQjdJDcE0hMf86j3m58BY7je5RK_AdbFU2egY");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage httpResponse = null;

            httpResponse = httpClient.GetAsync(url).Result;  //.PostAsync(url, httpContent);
            string resp = httpResponse.Content.ReadAsStringAsync().Result;
            using (OurDbContext db = new OurDbContext())
            {
                db.uniqueDetails.Add(unique);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.Message = unique.uniqueName + "successful registered";
            return View();
        }
    }
}