using Matslumpiator.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Matslumpiator.Models
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
        [Display(Name = "Betyg")]
        public double Rating { get; set; }
        [Display(Name = "Tillägsningstid")]
        public string cookingtime { get; set; }
        [Display(Name = "Huvudprotein")]
        public string TypeOfFood { get; set; }
        [Display(Name = "Tillfälle")]
        public string Occasions { get; set; }
        public string Weeknumbers { get; set; }
        public DateTime Date { get; set; }
        public List<Receptmodels> Recept { get; set; }

 

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
    }
}