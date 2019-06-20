using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using TopGear_WebApi_DependencyInjection2.Models;

namespace TopGear_WebApi_DependencyInjection2.Controllers
{

    public class PostsController : Controller
    {
        private const string BASE_ADDRESS = "http://localhost:50040/api/";

        private PostViewModel CreateVmFromDto(PostDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            PostViewModel vm = new PostViewModel()
            {
                Id = dto.Id,
                Title = dto.Title,
                Body = dto.Body,
                Time = dto.Time,
                UserName = dto.UserName
            };
            return vm;
        }

        private PostDTO GetPostDtoById(int id)
        {
            PostDTO post = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                // HTTP GET
                var responseTask = client.GetAsync("posts?id=" + id.ToString());
                responseTask.Wait();

                var response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<PostDTO>();
                    readTask.Wait();

                    post = readTask.Result;
                }
                else // web api sent error response
                {
                    /* TODO: Log response status here... */

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return post;
        }

        // GET: Posts
        public ActionResult Index()
        {
            IEnumerable<PostViewModel> posts = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);

                // HTTP GET
                var responseTask = client.GetAsync("posts");
                responseTask.Wait();

                var response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = response.Content.ReadAsAsync<IList<PostDTO>>();
                    readTask.Wait();

                    // Convert List of PostDTO, to List of PostViewModel
                    IList<PostViewModel> newList = new List<PostViewModel>();
                    foreach (PostDTO dto in readTask.Result)
                    {
                        newList.Add(CreateVmFromDto(dto));
                    }
                    posts = newList;
                }
                else // web api sent error response
                {
                    /* TODO: Log response status here... */

                    posts = Enumerable.Empty<PostViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(posts);
        }

        // GET: Post by Id
        public ActionResult Details(int id)
        {
            PostDTO dto = GetPostDtoById(id);
            PostViewModel vm = CreateVmFromDto(dto);
            return View(vm);
        }

        // GET: Create new post
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create new post
        [HttpPost]
        public ActionResult Create(PostViewModel vm)
        {
            // Construct DTO from VM; provide Time and User info
            PostDTO dto = new PostDTO()
            {
                Title = vm.Title,
                Body = vm.Body,
                Time = DateTime.Now,
                UserId = 5 // sample user
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);

                // HTTP POST
                var postTask = client.PostAsJsonAsync<PostDTO>("posts", dto);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            // Error
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(vm);
        }

        // GET: Edit post
        public ActionResult Edit(int id)
        {
            PostDTO dto = GetPostDtoById(id);
            PostViewModel vm = CreateVmFromDto(dto);
            return View(vm);
        }

        // PUT: Edit post
        [HttpPost]
        public ActionResult Edit(PostViewModel vm)
        {
            // Construct DTO from VM
            PostDTO dto = new PostDTO()
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);

                // HTTP PUT
                var putTask = client.PutAsJsonAsync<PostDTO>("posts", dto);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            // Error
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(vm);
        }

        // GET: Delete post
        public ActionResult Delete(int id)
        {
            PostDTO dto = GetPostDtoById(id);
            PostViewModel vm = CreateVmFromDto(dto);
            return View(vm);
        }

        // DELETE: Delete post
        [HttpPost]
        public ActionResult Delete(PostViewModel vm)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);

                // HTTP DELETE
                var deleteTask = client.DeleteAsync("posts/" + vm.Id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}