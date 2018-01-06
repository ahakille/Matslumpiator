using Matslump.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Matslump.Models
{
    public class Receptmodels
    {
        public int Id { get; set; }
        [Display(Name = "Namnet")]
        [Required]
        public string Name { get; set; }        
        [Display(Name = "Bild")]
        public string Url_pic { get; set; }
        [Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Url till receptet")]
        public string Url_recept { get; set; }
        public string Weeknumbers { get; set; }
        public DateTime Date { get; set; }
        public List<Receptmodels> Recept { get; set; }

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
        public List<Receptmodels> SearchFood(string psql, int pid_user, string search)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<Receptmodels> mt = new List<Receptmodels>();
            string modal = "%";
            dt = m.SqlQuery(psql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id_user", pid_user),
                new NpgsqlParameter("@search",modal += search +="%")
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
        //public List<Receptmodels> getrecept(string id)
        //{
        //    postgres m = new postgres();
        //    DataTable dt = new DataTable();
        //    List<Receptmodels> mt = new List<Receptmodels>();
        //    dt = m.SqlQuery("SELECT * FROM recept WHERE id_recept =@id", postgres.list = new List<NpgsqlParameter>()
        //{
        //       new NpgsqlParameter("@id", id)
        //});
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string inn, doo;
        //        Receptmodels r = new Receptmodels();
        //        r.Id = Convert.ToInt16(dr["id_recept"].ToString());
        //        r.name = dr["name"].ToString();
        //        inn = dr["inn"].ToString();
        //        doo = dr["doo"].ToString();
        //        r.ind = inn.Split(',').ToList();
        //        r.doing = doo.Split(',').ToList();
        //        postgres m1 = new postgres();
        //        DataTable dt2 = new DataTable();
        //        List<keyword> lista = new List<keyword>();
        //        dt2 = m1.SqlQuery("SELECT * FROM keyword WHERE id_keyword IN(SELECT keyword_id FROM recept_has_keyword WHERE recept_id = @id)", postgres.list = new List<NpgsqlParameter>()
        //    {
        //           new NpgsqlParameter("@id",r.Id)
        //    });
        //        foreach (DataRow dr1 in dt2.Rows)
        //        {
        //            keyword k = new keyword();
        //            k.id = Convert.ToInt16(dr1["id_keyword"].ToString());
        //            k.name = dr1["keyword"].ToString();
        //            lista.Add(k);
        //        }
        //        r.listaKeyword = lista;
        //        mt.Add(r);
        //    }

        //    return mt;
        //}
        public void addNewFood(string pname,string des,string url_pic,string url_recept, int user_id)
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
        public void EditFood(int recept_id, string pname , string des,string url_pic,string url_recept)
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