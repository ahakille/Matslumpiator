using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Matslump.Models
{
    public class Users
    {
        public int User_id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Användarnamn")]
        public string User { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Aktivt konto")]
        public bool active { get; set; }
        [Display(Name = "Roll")]
        public int Roles_id { get; set; }
        [Display(Name = "Senast inloggad")]
        public DateTime Last_login { get; set; }

        public List<Users> Getuser(int id , string sql)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<Users> mt = new List<Users>();
            dt = m.SqlQuery(sql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id)
            });
            foreach (DataRow dr in dt.Rows)
            {
               
                Users r = new Users();
                r.User_id = (int)dr["user_id"];
                r.User = dr["username"].ToString();
                r.email = (string)dr["email"];
                r.active = (bool)dr["acc_active"];
                r.Roles_id = (int)dr["roles_id"];
                r.Last_login = (DateTime)dr["last_login"];


                mt.Add(r);
            }

            return mt;
        }

    }
}