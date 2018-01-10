using Matslump.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace Matslump.Models
{
    public class UsersEditViewmodel
    {
        public int User_id { get; set; }
        [Display(Name = "Användarnamn")]
        public string User { get; set; }
        [Required]
        [Display(Name = "Förnamn")]
        public string First_name { get; set; }
        [Required]
        [Display(Name = "Efternamn")]
        public string Last_name { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string email { get; set; }
        [Display(Name = "Dag för automatisk slumpning")]
        public int CronoDay { get; set; }
    }
    public class Users
    {
        public int User_id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "Användarnamn")]
        public string User { get; set; }

        public string First_name { get; set; }
        public string Last_name { get; set; }
  

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta Lösenord")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Aktivt konto")]
        public bool active { get; set; }
        [Display(Name = "Roll")]
        public int Roles_id { get; set; }
        [Display(Name = "Senast inloggad")]
        public DateTime Last_login { get; set; }
        public int Settings_id { get; set; }
        [Display(Name = "Dag för automatisk slumpning")]
        public int CronoDay { get; set; }

        public int Login_id { get; set; }

        public List<Users> GetuserAsAdmin(int id , string sql)
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
                r.Last_name = (string)dr["last_name"];
                r.First_name = (string)dr["fname"];
                r.Settings_id = (int)dr["settings_id"];


                mt.Add(r);
            }

            return mt;
        }
        public List<UsersEditViewmodel> Getuser(int id, string sql)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<UsersEditViewmodel> mt = new List<UsersEditViewmodel>();
            dt = m.SqlQuery(sql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id)
            });
            foreach (DataRow dr in dt.Rows)
            {

                UsersEditViewmodel r = new UsersEditViewmodel();
                r.User_id = (int)dr["user_id"];
                r.User = dr["username"].ToString();
                r.email = (string)dr["email"];
                r.First_name = (string)dr["fname"];
                r.Last_name = (string)dr["last_name"];
                r.CronoDay = (int)dr["day_of_slumpcron"];



                mt.Add(r);
            }

            return mt;
        }
        public void CreateUser(string user,string email,bool active,string Password,string fname,string last_name)
        {
            Accountservice User = new Accountservice();
            Tuple<byte[], byte[]> password = User.Generatepass(Password);
            postgres sql = new postgres();
            // Behöver skrivas om! klart!
            postgres sql2 = new postgres();
            
            int id = sql2.SqlQueryString("INSERT INTO login (salt, hash, reset_time, reset_hash) VALUES (@salt ,@hash, @time, 1) RETURNING login_id;", postgres.list = new List<NpgsqlParameter>()
            {
                
                new NpgsqlParameter("@salt", password.Item1),
                new NpgsqlParameter("@hash", password.Item2),
                new NpgsqlParameter("@time", Convert.ToDateTime("1970-01-01 00:00:00"))
            });
            postgres sql3 = new postgres();
            int id_setting = sql3.SqlQueryString("INSERT INTO usersettings (day_of_slumpcron) VALUES (6) RETURNING setting_id;", postgres.list = new List<NpgsqlParameter>()
            { 
            });

            sql.SqlNonQuery("INSERT INTO users (username,roles_id,email,acc_active,last_login,login_id,settings_id,fname,last_name) VALUES (@par1,'2',@email,@active,@last_login,@login_id,@settings_id,@fname,@last_name)", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", user),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@active", active),
                new NpgsqlParameter("@login_id", id),
                new NpgsqlParameter("@settings_id", id_setting),
                new NpgsqlParameter("@last_login", DateTime.Now),
                new NpgsqlParameter("@fname", fname),
                new NpgsqlParameter("@last_name", last_name)

            });
       


            
        }
        public void UpdateUser(int User_id,string username,string email,string first_name, string last_name,int Slumpday)
        {
            postgres sql = new postgres();
            //Behöver skrivas OM! klar
            sql.SqlNonQuery("Select update_user(@username,@email,@first_name,@lastname,@cronoday,@user_id)", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@first_name", first_name),
                new NpgsqlParameter("@lastname", last_name),
                new NpgsqlParameter("@cronoday", Slumpday),
                new NpgsqlParameter("@user_id", User_id)
              
               
            });
        }
        public void Newpassword(int login_id,string newpassword)
        {
            Accountservice User1 = new Accountservice();
            Tuple<byte[], byte[]> password = User1.Generatepass(newpassword);
            postgres sql = new postgres();
            // behöver skrivas om! klart
            sql.SqlNonQuery("UPDATE login set salt= @par2, hash =@par3 WHERE login_id =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", login_id),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });
        }


    }
}