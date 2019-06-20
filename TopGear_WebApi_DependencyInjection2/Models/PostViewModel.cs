using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TopGear_WebApi_DependencyInjection2.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
    }
}