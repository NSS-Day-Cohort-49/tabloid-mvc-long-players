using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Post> GetTagById();
        void Add(Tag Tag);
    }
}
