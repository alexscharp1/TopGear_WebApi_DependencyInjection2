//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PostName { get; set; }
        public string PostBody { get; set; }
        public System.DateTime Time { get; set; }
    
        public virtual User User { get; set; }
    }
}
