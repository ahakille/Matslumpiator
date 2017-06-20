using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
        [Display(Name = "Registeringskod")]
        public string Secret { get; set; }

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
        public void CreateUser(string user,string email,bool active,string Password)
        {
            Accountmodels User = new Accountmodels();
            Tuple<byte[], byte[]> password = User.Generatepass(Password);
            postgres sql = new postgres();
            sql.SqlNonQuery("INSERT INTO login (salt, key ,username,roles_id,email,acc_active,last_login) VALUES (@par2,@par3,@par1,'2',@email,@active,@last_login)", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", user),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@active", active),
                new NpgsqlParameter("@last_login", DateTime.Now),
                new NpgsqlParameter("@par3", password.Item2)
            });
        }
        public void UpdateUser(int User_id,string username,string email,bool active)
        {
            postgres sql = new postgres();
            sql.SqlNonQuery("UPDATE login SET username=@username, email=@email,acc_active=@active WHERE user_id=@user_id", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@active", active),
                new NpgsqlParameter("@user_id", User_id)
              
               
            });
        }


    }
}