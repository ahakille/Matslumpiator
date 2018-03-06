using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Matslump.Services
{
    public class Foodservices
    {
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

    }
}