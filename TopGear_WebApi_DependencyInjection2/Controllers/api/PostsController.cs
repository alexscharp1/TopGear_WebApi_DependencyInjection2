using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TopGear_WebApi_DependencyInjection2.Models;

namespace TopGear_WebApi_DependencyInjection2.Controllers.api
{
    public class PostsController : ApiController
    {
        private WebApiDependencyInjectionDBEntities1 db = new WebApiDependencyInjectionDBEntities1();

        // GET: api/Posts
        public IQueryable<PostDTO> GetPosts()
        {
            var posts = from p in db.Posts
                        select new PostDTO()
                        {
                            Id = p.Id,
                            Title = p.PostName,
                            Body = p.PostBody,
                            Time = p.Time,
                            UserName = p.User.FirstName + " " + p.User.LastName,
                            UserId = p.UserId
                        };
            return posts;
        }

        // GET: api/Posts/5
        [ResponseType(typeof(PostDTO))]
        public async Task<IHttpActionResult> GetPost(int id)
        {
            var post = await db.Posts.Include(p => p.User).Select
                            (p => new PostDTO()
                            {
                                Id = p.Id,
                                Title = p.PostName,
                                Body = p.PostBody,
                                Time = p.Time,
                                UserName = p.User.FirstName + " " + p.User.LastName,
                                UserId = p.UserId
                            }).SingleOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPost(PostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = await db.Posts.FindAsync(dto.Id);
            if (post == null)
            {
                return NotFound();
            }

            try
            {
                post.PostName = dto.Title;
                post.PostBody = dto.Body;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(dto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Posts
        [ResponseType(typeof(PostDTO))]
        public async Task<IHttpActionResult> PostNewPost(PostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = new Post()
            {
                UserId = dto.UserId,
                PostName = dto.Title,
                PostBody = dto.Body,
                Time = dto.Time
            };
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dto.Id }, dto);
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> DeletePost(int id)
        {
            Post post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            await db.SaveChangesAsync();

            return Ok(new PostDTO()
            {
                Id = post.Id,
                Title = post.PostName,
                Body = post.PostBody,
                Time = post.Time,
                UserName = post.User.FirstName + " " + post.User.LastName,
                UserId = post.UserId,
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.Id == id) > 0;
        }
    }
}