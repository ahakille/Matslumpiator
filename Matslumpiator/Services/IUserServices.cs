using Matslumpiator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public interface IUserServices
    {
        List<UserService> GetuserAsAdmin(int id, string sql);
        List<UsersEditViewmodel> Getuser(int id, string sql);
        void UpdateUser(int User_id, string username, string email, string first_name, string last_name, int Slumpday);
        void Newpassword(int login_id, string newpassword);
        Task SendMessagesToAllUsers(string subject, string message);
        void DeleteUser(int User_id);
    }
}
