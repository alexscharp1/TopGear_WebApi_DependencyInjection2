using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class EmailConfig
    {
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string Host { get; set; }
    }
}