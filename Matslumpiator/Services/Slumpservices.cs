﻿using Matslumpiator.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Matslumpiator.Services
{
    public class Slumpservices: ISlumpServices
    {
        private readonly IFoodServices _foodServices;

        public Slumpservices(IFoodServices foodServices)
        {
            _foodServices = foodServices;
        }
        public List<Receptmodels> CreateRandomListOfRecept()
        {
            var List = _foodServices.GetFoodListForReceptView("SELECT * FROM public.recept_search_view", 1, null);
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

        public string NameOfDay(DateTime date)
        {
            var DateEN = date.DayOfWeek.ToString();
            string DateSv ="";
            switch (DateEN)
            {
                case "Monday":
                    DateSv = "Måndag";
                    break;
                case "Tuesday":
                    DateSv = "Tisdag";
                    break;
                case "Wednesday":
                    DateSv = "Onsdag";
                    break;
                case "Thursday":
                    DateSv = "Torsdag";
                    break;
                case "Friday":
                    DateSv = "Fredag";
                    break;
                case "Saturday":
                    DateSv = "Lördag";
                    break;
                case "Sunday":
                    DateSv = "Söndag";
                    break;
            }

            return DateSv;
        }
        public List<Receptmodels> Slumplist(int user_id, DateTime date)
        {
            date = datefixer(date);
            List<Receptmodels> food_list = new List<Receptmodels>();
            Receptmodels re = new Receptmodels();
            food_list = _foodServices.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", user_id);
            int maxdays = 5;
            int maxnumber = food_list.Count;
            Random rnd = new Random();
            List<Receptmodels> random_list = new List<Receptmodels>();
            int i = 0;
            bool check = true;
            if (maxnumber > 7)
            {
                while (i < maxdays)
                {
                    check = true;
                    int number = rnd.Next(1, maxnumber);


                    foreach (var item in random_list)
                    {
                        if (item.Id == food_list[number].Id)
                        {
                            check = false;
                        }
                    }
                    if (check)
                    {
                        food_list[number].Date = date;
                        food_list[number].DateName = NameOfDay(date);
                        random_list.Add(food_list[number]);
                        i++;
                        date = date.AddDays(1);
                    }



                }
            }
            if (maxnumber <= 7)
            {
                re.Id = -10;
                re.Name = "Inte tillräckligt med maträtter för att slumpa fram en vecka!!!";
                random_list.Add(re);
            }



            return random_list;
        }
        public DateTime datefixer(DateTime date)
        {
            int test = ((int)date.DayOfWeek == 0) ? 7 : (int)date.DayOfWeek;
            test--;
            date = date.AddDays(-test);
            return date;
        }
        public int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public bool Checkslump(DateTime date, int user_id)
        {
            postgres m = new postgres();
            bool check = false;
            DataTable dt = new DataTable();
            dt = m.SqlQuery("SELECT EXISTS(SELECT foodlist.date_now,foodlist.recept_id FROM public.foodlist Where foodlist.user_id = @user_id AND date_now = @date_now)", postgres.list = new List<NpgsqlParameter>()
        {

               new NpgsqlParameter("@date_now", date),
               new NpgsqlParameter("@user_id", user_id)

        });
            foreach (DataRow dr in dt.Rows)
            {
                check = (bool)dr["exists"];
            }
            return check;
        }
        public void SaveSlump(int recept_id, int user_id, DateTime date, bool check)
        {
            date = date.Date;
            string sql = "INSERT INTO foodlist (user_id,recept_id,date_now) values(@user_id,@recept_id,@date_now)";
            if (check)
            {
                sql = "UPDATE foodlist SET recept_id = @recept_id WHERE date_now=@date_now AND user_id = @user_id";
            }
            postgres m = new postgres();
            m.SqlNonQuery(sql, postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@recept_id", recept_id),
               new NpgsqlParameter("@date_now", date),
               new NpgsqlParameter("@user_id", user_id)

        });
        }
        public List<Receptmodels> Oldslumps(int user_id, DateTime date, DateTime dateto)
        {


          //  Slump Slump = new Slump();
            postgres m = new postgres();
            List<Receptmodels> mt = new List<Receptmodels>();
            DataTable dt = new DataTable();
            dt = m.SqlQuery("SELECT foodlist.date_now,foodlist.recept_id,recept.name,recept.description,recept.url_pic,recept.url_recept, cookingtime.time_name, type_of_food.type_name, recept.average_rating,recept.occasion_id FROM public.foodlist join public.recept on foodlist.recept_id = recept.id_recept LEFT JOIN type_of_food ON recept.type_of_food_id = type_of_food.id LEFT JOIN cookingtime ON recept.cookingtime_id = cookingtime.id Where foodlist.user_id = @id_user AND date_now BETWEEN @datefrom AND @dateto ORDER BY date_now DESC; ", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id_user", user_id),
                new NpgsqlParameter("@dateto", dateto),
                new NpgsqlParameter("@datefrom", date)
            });
            foreach (DataRow dr in dt.Rows)
            {

                Receptmodels r = new Receptmodels();
                r.Id = (int)dr["recept_id"];
                r.Name = dr["name"].ToString();
                r.Date = (DateTime)dr["date_now"];
                r.Description = (string)dr["description"];
                r.Url_pic = (string)dr["url_pic"];
                r.Url_recept = (string)dr["url_recept"];
                r.cookingtime = (string)dr["time_name"];
                r.TypeOfFood = (string)dr["type_name"];
                r.Occasions = dr["occasion_id"].ToString();
                r.Rating = (double)dr["average_rating"];
  //              var dateName = new Slumpservices();
                r.DateName = NameOfDay(r.Date);
                r.Weeknumbers = GetIso8601WeekOfYear(r.Date).ToString();


                mt.Add(r);
            }






            return mt;
        }
        public List<Slump> Weeknumbers(List<Receptmodels> lista)
        {

            List<Slump> list = new List<Slump>();
            string check = "";
            foreach (var item in lista)
            {
                if (check != item.Weeknumbers)
                {
                    Slump sl = new Slump();
                    sl.Weeknumber = item.Weeknumbers;
                    list.Add(sl);
                    check = item.Weeknumbers;
                }

            }
            return list;

        }
    }
}