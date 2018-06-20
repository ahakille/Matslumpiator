using Matslumpiator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public interface IFoodServices
    {
        string CreateSearchString(bool chicken, bool vego, bool fish, bool beef, bool pork, bool Sausage, bool meat, bool other, string search, string cookingtime);
        List<Receptmodels> GetFoodListForReceptView(string psql, int pid_user, string search);
        List<Receptmodels> GetFood(string psql, int pid_user);
        void AddNewFood(string pname, string des, string url_pic, string url_recept, int user_id);
        void EditFood(int recept_id, string pname, string des, string url_pic, string url_recept);
        void AddFoodToUser(string id_user, int id_recept);
        void RemovefoodFromUser(string id_user, int id_recept);
    }
}
