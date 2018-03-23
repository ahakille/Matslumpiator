using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matslump.Services
{
    public class Slumpservices
    {
        public List<Receptmodels> CreateRandomListOfRecept()
        {
            var Foodlist = new Foodservices();
            var List =  Foodlist.GetFoodListForReceptView("SELECT * FROM public.recept_search_view", 1, null);
            Random rnd = new Random();
            int maxnumber = List.Count;
            var ListOfrandom = new List<Receptmodels>();
            for (int i = 0; i < 3; i++)
            {
                int number = rnd.Next(1, maxnumber);
                ListOfrandom.Add(List[number]);
            }
            return ListOfrandom;

        }
    }
}