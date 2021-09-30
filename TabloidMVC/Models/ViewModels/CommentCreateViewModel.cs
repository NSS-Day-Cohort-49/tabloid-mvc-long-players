using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public Comment Comment { get; set; }
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public UserProfile UserProfile { get; set; }

        public int PostId { get; set; }
    }
}
