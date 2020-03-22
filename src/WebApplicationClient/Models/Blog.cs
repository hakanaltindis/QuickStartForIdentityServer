using System;
using System.Collections.Generic;

namespace WebApplicationClient.Models
{
    public class Blog
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }
}
