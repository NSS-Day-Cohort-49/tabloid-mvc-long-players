using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {

        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        UserProfile GetUserProfileById(int Id);
        void Add(UserProfile userProfile);
        void UpdateUserTypeAdmin(UserProfile userProfile);
        void UpdateUserTypeAuthor(UserProfile userProfile);
        //void Delete(UserProfile userProfile);
        void Update(UserProfile userProfile);
        void UpdateStatus(UserProfile userProfile);
        void ActivateStatus(UserProfile userProfile);
        void Delete(int userProfileId);
      
    }
}