using Matslumpiator.Models;
using System;
using System.Collections.Generic;

namespace Matslumpiator.Services
{
    public class Slumpservices
    {
        public List<Receptmodels> CreateRandomListOfRecept()
        {
            var Foodlist = new Foodservices();
            var List =  Foodlist.GetFoodListForReceptView("SELECT recept.id_recept, recept.name, recept.description,recept.url_pic,recept.url_recept,recept.cookingtime,type_of_food.type_name,recept.average_rating,recept.occasion_id  FROM recept LEFT JOIN type_of_food ON recept.type_of_food_id = type_of_food.id", 1, null);
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