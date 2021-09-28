using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Post post { get; set; }
        public List<Tag> Tags { get; set; }

        //comments will go here, I'm assuming this is also a list
    }
}
