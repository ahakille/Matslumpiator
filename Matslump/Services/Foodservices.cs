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
                r.cookingtime = (string)dr["cookingtime"];
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