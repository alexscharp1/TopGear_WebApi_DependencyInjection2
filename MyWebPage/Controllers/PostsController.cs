using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MyWebPage.Models;
using MyWebPage.Filters;
using System.Net.Http.Headers;
using System.Text;

namespace MyWebPage.Controllers
{
    public class PostsController : Controller
    {
        private const string BASE_ADDRESS = "http://localhost:54637/";
        TokenViewModel token;

        private PostViewModel GetPostById(int id)
        {
            PostViewModel post = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);

                // HTTP GET
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();

                var response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<PostViewModel>();
                    readTask.Wait();

                    post = readTask.Result;
                }
                else // Web api sent error response
                {
                    /* TODO: Logging */

                    ModelState.AddModelError(string.Empty, "Server error.");
                }
            }

            return post;
        }

        // GET: Posts/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Posts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel2 model, string returnUrl)
        {
            // JSON object to be sent to API for authentication
            var obj = new
            {
                grant_type = "password",
                username = model.Email,
                password = model.Password
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "token");
                request.Content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                var responseTask = client.SendAsync(request);

                // HTTP POST
                //var responseTask = client.PostAsJsonAsync("token", json);
                responseTask.Wait();

                var response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<TokenViewModel>();
                    readTask.Wait();

                    token = readTask.Result;
                }
                else // Web api sent error response
                {
                    /* TODO: Logging */

                    ModelState.AddModelError(string.Empty, "Server error.");
                }
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Posts
        public ActionResult Index()
        {
            if (token == null)
            {
                return Redirect("Login");
            }

            IEnumerable<PostViewModel> posts = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Authorization", "Bearer " + token.access_token);

                // HTTP GET
                var responseTask = client.GetAsync("api/posts");
                responseTask.Wait();

                var response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<IList<PostViewModel>>();
                    readTask.Wait();

                    posts = readTask.Result;
                }
                else // Web api sent error response
                {
                    /* TODO: Logging */

                    posts = Enumerable.Empty<PostViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error.");
                }
            }

            return View(posts);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int id)
        {
            PostViewModel post = GetPostById(id);
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var post = new
                {
                    UserId = 5, // TODO: Temp value for sample user
                    PostTitle = collection["PostTitle"],
                    PostBody = collection["PostBody"],
                    Time = DateTime.Now
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_ADDRESS);

                    // HTTP POST
                    var postTask = client.PostAsJsonAsync("api/posts", post);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server Error.");
                        return View();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server Error.");
                return View();
            }
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Posts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
