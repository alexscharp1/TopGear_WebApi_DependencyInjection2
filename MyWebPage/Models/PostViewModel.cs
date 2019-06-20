using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyWebPage.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string PostName { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string PostBody { get; set; }

        public DateTime Time { get; set; }
    }
}