using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {

        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        //UserProfile GetUserProfileById(int Id);
        //void Add(UserProfile userProfile);
        //void Update(UserProfile userProfile);
        //void Delet(UserProfile userProfile);

    }
}