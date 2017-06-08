using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Matslump.Models
{
    public class Receptmodels
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<keyword> listaKeyword { get; set; }
        public List<string> ind { get; set; }
        [Display(Name = "De här behövs")]
        public string indstring { get; set; }
        public List<string> doing { get; set; }
        [Display(Name = "Så här gör du ")]
        public string doingstring { get; set; }
        public DateTime date { get; set; }
        public List<Receptmodels> recept { get; set; }

        public List<Receptmodels> getFood(string psql, int pid_user)
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
                string test;
                Receptmodels r = new Receptmodels();
                r.id = Convert.ToInt16(dr["id_recept"].ToString());
                r.name = dr["name"].ToString();
                

                mt.Add(r);
            }

            return mt;
        }
        public List<Receptmodels> getrecept(string id)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<Receptmodels> mt = new List<Receptmodels>();
            dt = m.SqlQuery("SELECT * FROM recept WHERE id_recept =@id", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id", id)
        });
            foreach (DataRow dr in dt.Rows)
            {
                string inn, doo;
                Receptmodels r = new Receptmodels();
                r.id = Convert.ToInt16(dr["id_recept"].ToString());
                r.name = dr["name"].ToString();
                inn = dr["inn"].ToString();
                doo = dr["doo"].ToString();
                r.ind = inn.Split(',').ToList();
                r.doing = doo.Split(',').ToList();
                postgres m1 = new postgres();
                DataTable dt2 = new DataTable();
                List<keyword> lista = new List<keyword>();
                dt2 = m1.SqlQuery("SELECT * FROM keyword WHERE id_keyword IN(SELECT keyword_id FROM recept_has_keyword WHERE recept_id = @id)", postgres.list = new List<NpgsqlParameter>()
            {
                   new NpgsqlParameter("@id",r.id)
            });
                foreach (DataRow dr1 in dt2.Rows)
                {
                    keyword k = new keyword();
                    k.id = Convert.ToInt16(dr1["id_keyword"].ToString());
                    k.name = dr1["keyword"].ToString();
                    lista.Add(k);
                }
                r.listaKeyword = lista;
                mt.Add(r);
            }

            return mt;
        }
        public void addNewFood(string pname, List<string> doolist , List<string> innlist, int user_id)
        {
            string doo = String.Join(",", doolist);
            string inn = String.Join(",", innlist);
            //Fullösning
            postgres m = new postgres();
            m.SqlNonQuery("INSERT INTO recept (name,inn,doo, created_by_user) values(@name,@inn,@doo,@user_id)", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@name", pname),
               new NpgsqlParameter("@inn", inn),
               new NpgsqlParameter("@doo", doo),
               new NpgsqlParameter("@user_id", user_id)

        });
        }
        public void addFood_user(string id_user, int id_recept)
        {
            postgres m = new postgres();
            m.SqlNonQuery("INSERT INTO users_has_recept (recept_id ,user_id) values(@id_recept, @id_user)", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_user", Convert.ToInt16(id_user)),
               new NpgsqlParameter("@id_recept", id_recept)
        });
        }
        public void removefood_user(string id_user, int id_recept)
        {
            postgres m = new postgres();
            m.SqlNonQuery("DELETE FROM users_has_recept where recept_id =@id_recept AND user_id = @id_user", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_user",Convert.ToInt16(id_user)),
               new NpgsqlParameter("@id_recept",id_recept)
        });
        }
        public void addKeywordToFood(int id_recept, int id_keyword)
        {
            postgres m = new postgres();
            m.SqlNonQuery("INSERT INTO recept_has_keyword (keyword_id,recept_id) values(@id_recept, @id_keyword)", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_keyword", id_keyword),
               new NpgsqlParameter("@id_recept", id_recept)
        });
        }
        public void removeKeywordFromFood(int id_keyword, int id_recept)
        {
            postgres m = new postgres();
            m.SqlNonQuery("DELETE FROM recept_has_keyword WHERE recept_id =@id_recept AND keyword_id = @id_keyword", postgres.list = new List<NpgsqlParameter>()
        {
               new NpgsqlParameter("@id_keyword", id_keyword),
               new NpgsqlParameter("@id_recept", id_recept)
        });
        }
    }
}