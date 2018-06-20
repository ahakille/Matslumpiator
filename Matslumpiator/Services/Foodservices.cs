using Matslumpiator.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Matslumpiator.Services
{
    public class Foodservices:IFoodServices
    {
        public Foodservices()
        {

        }
        public List<Receptmodels> ReceptList { get; set; }

        public string CreateSearchString(bool chicken , bool vego , bool fish , bool beef , bool pork, bool Sausage , bool meat , bool other , string search, string cookingtime)
        {
            string sql ="";
            bool onemore= false;
            bool twomore = false;
            if (chicken || vego || fish  || beef || pork || Sausage||meat||other|| !string.IsNullOrEmpty(search)||cookingtime !="Över 60 minuter" )
            {
                sql = " WHERE ";
                if ((chicken || vego || fish || beef || pork || Sausage || meat || other)&& !string.IsNullOrEmpty(search)|| (chicken || vego || fish || beef || pork || Sausage || meat || other) && cookingtime != "Över 60 minuter")
                {
                    sql += "( ";
                }
                if (chicken)
                {
                    onemore = true;
                    sql += "type_name = 'Kyckling'";
                }
                if (vego)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Vegetarisk'";
                }
                if (fish)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Fisk'";
                }
                if (beef)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Nöt'";
                }
                if (pork)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Fläsk'";
                }
                if (Sausage)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Korv'";
                }
                if (meat)
                {
                    if (onemore)
                    {
                        sql += " OR ";
                    }
                    onemore = true;
                    sql += "type_name = 'Kött'";
                }
                if (cookingtime != "Över 60 minuter")
                {
                    twomore = true;
                    if (onemore)
                    {
                        sql += ") AND ";
                    }
                    sql += "time_name = '"+cookingtime+"'";
                }
                if (!string.IsNullOrEmpty(search))
                {
                    if (onemore && !twomore )
                    {
                        sql += ") AND ";
                    }
                    if (twomore)
                    {
                        sql += " AND ";
                    }
                    sql += "name LIKE @search";
                }
            }
           
            return sql;
        }
        public List<Receptmodels> GetFoodListForReceptView(string psql, int pid_user,string search)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<Receptmodels> mt = new List<Receptmodels>();
            if (!string.IsNullOrEmpty(search))
            {
                string modal = "%";
                dt = m.SqlQuery(psql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id_user", pid_user),
                new NpgsqlParameter("@search",modal += search +="%")
            });
            }
            else
            {
                dt = m.SqlQuery(psql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id_user", pid_user)
            });
            }

            foreach (DataRow dr in dt.Rows)
            {

                Receptmodels r = new Receptmodels();
                r.Id = Convert.ToInt16(dr["id_recept"].ToString());
                r.Name = dr["name"].ToString();
                r.Description = (string)dr["description"];
                r.Url_pic = (string)dr["url_pic"];
                r.Url_recept = (string)dr["url_recept"];
                r.cookingtime = (string)dr["time_name"];
                r.TypeOfFood = (string)dr["type_name"];
                r.Occasions = dr["occasion_id"].ToString();
                r.Rating = (double)dr["average_rating"];



                mt.Add(r);
            }

            return mt;
        }
        public List<Receptmodels> GetFood(string psql, int pid_user)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<Receptmodels> mt = new List<Receptmodels>();
            dt = m.SqlQuery(psql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id_user", pid_user)
            });
            foreach (DataRow dr in dt.Rows)
            {

                Receptmodels r = new Receptmodels();
                r.Id = Convert.ToInt16(dr["id_recept"].ToString());
                r.Name = dr["name"].ToString();
                r.Description = (string)dr["description"];
                r.Url_pic = (string)dr["url_pic"];
                r.Url_recept = (string)dr["url_recept"];



                mt.Add(r);
            }

            return mt;
        }

        public void AddNewFood(string pname, string des, string url_pic, string url_recept, int user_id)
        {


            postgres m = new postgres();
            m.SqlNonQuery("INSERT INTO recept (name,description, created_by_user, url_pic,url_recept) values(@name,@description,@user_id,@url_pic,@url_recept)", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@name", pname),
               new NpgsqlParameter("@description", des),
               new NpgsqlParameter("@url_pic", url_pic),
               new NpgsqlParameter("@url_recept", url_recept),
               new NpgsqlParameter("@user_id", user_id)

        });
        }
        public void EditFood(int recept_id, string pname, string des, string url_pic, string url_recept)
        {


            postgres m = new postgres();
            m.SqlNonQuery("UPDATE recept SET name = @name ,description = @description, url_pic =@url_pic,url_recept=@url_recept WHERE id_recept = @recept_id", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@recept_id", recept_id),
               new NpgsqlParameter("@name", pname),
               new NpgsqlParameter("@url_pic", url_pic),
               new NpgsqlParameter("@url_recept", url_recept),
               new NpgsqlParameter("@description", des)


        });
        }
        public void AddFoodToUser(string id_user, int id_recept)
        {
            postgres m = new postgres();
            m.SqlNonQuery("INSERT INTO users_has_recept (recept_id ,user_id) values(@id_recept, @id_user)", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_user", Convert.ToInt16(id_user)),
               new NpgsqlParameter("@id_recept", id_recept)
        });
        }
        public void RemovefoodFromUser(string id_user, int id_recept)
        {
            postgres m = new postgres();
            m.SqlNonQuery("DELETE FROM users_has_recept where recept_id =@id_recept AND user_id = @id_user", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_user",Convert.ToInt16(id_user)),
               new NpgsqlParameter("@id_recept",id_recept)
        });
        }

    }
}