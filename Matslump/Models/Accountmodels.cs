using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

namespace Matslump.Models
{
    public class Accountmodels
    {
        [Required]
        [Display(Name = "Användarnamn")]
        // [EmailAddress]
        public string user { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        public Tuple<int,bool,string> AuthenticationUser(string ppassword, string userid)
        {

            byte[] salt = null, key = null;
            int user_id =0;
            postgres m = new postgres();
            string role= "";
            bool active;
            // Behöver skrivas om! klar
           var dt = m.SqlQuery("select user_id, name, acc_active , login.salt , login.hash from users LEFT JOIN roles ON roles_id = id_roles LEFT JOIN login ON users.login_id = login.login_id where username =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", userid),

            });
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    salt = (byte[])dr["salt"];
                    key = (byte[])dr["hash"];
                    user_id = (int)dr["user_id"];
                    role = (string)dr["name"];
                    active = (bool)dr["acc_active"];

                }
            }
            catch
            {

            }

            if (salt != null)
            {
                using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, salt))
                {
                    byte[] newKey = deriveBytes.GetBytes(192);
                    if (!newKey.SequenceEqual(key))
                    {
                        return Tuple.Create(user_id, false, role);
                    }
                    else
                    {
                        postgres p = new postgres();
                        p.SqlNonQuery("UPDATE public.users SET last_login=@d WHERE user_id = @user_id;", postgres.list = new List<NpgsqlParameter>()
                        {
                         new NpgsqlParameter("@d", DateTime.Now),
                         new NpgsqlParameter("@user_id", user_id)
                         });
                        return Tuple.Create(user_id, true, role);
                    }

                }
            }
            else
            {
                return Tuple.Create(user_id, false,role);
            }

        }
        public Tuple<byte[], byte[]> Generatepass(string ppassword)
        {
            // Genererar en 192-byte salt
            using (var deriveBytes = new Rfc2898DeriveBytes(ppassword, 192))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(192);  // Skapar 192-byte key av lösenordet

                return Tuple.Create(salt, key);

            }
        }
        public string GeneratePassword(int p)
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= p; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }
        public void RegisterNewUser(string username, string email)
        {
            
            string pass = GeneratePassword(7);
            Users us = new Users();
            us.CreateUser(username, email, true, pass);
            Email.SendEmail(email, username, "Ditt lösenord", Email.EmailOther("Ditt lösenord är: ",pass));
        }
        public bool Forgetpassword(string username)
        {
            postgres sql = new postgres();
            DataTable dt = new DataTable();
            dt = sql.SqlQuery("SELECT login_id, username, email, acc_active FROM users WHERE username = @username;", postgres.list = new List<NpgsqlParameter>()
                        {
                         new NpgsqlParameter("@username", username)
                         });
            Users r = new Users();
            foreach (DataRow dr in dt.Rows)
            {

                
                
                r.User = dr["username"].ToString();
                r.email = (string)dr["email"];
                r.active = (bool)dr["acc_active"];
                r.Login_id = (int)dr["login_id"];
            }

                
            if (string.IsNullOrEmpty(r.User) || !r.active )
            {
                return false;
            }
            string hash = GeneratePassword(60);
            postgres sql1 = new postgres();
            sql1.SqlNonQuery("UPDATE login set reset_hash=@hash , reset_time =@time WHERE login_id=@id  ", postgres.list = new List<NpgsqlParameter>()
                        {
                         new NpgsqlParameter("@id",r.Login_id),
                         new NpgsqlParameter("@hash",hash),
                         new NpgsqlParameter("@time",DateTime.Now)
                         });
            string message =Email.EmailOther(r.User, "Här kommer din återställningslänk: <a href=\"https://matslumpiator.se/validate=" + hash + "\" target=\"_blank\" >Återställlösenorder </a>");
            Email.SendEmail(r.email, r.User, "Återställning Lösenord", message);
            return true;
            
        }
    }
}