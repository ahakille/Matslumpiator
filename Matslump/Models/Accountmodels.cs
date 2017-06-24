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

           var dt = m.SqlQuery("select salt, key, user_id, name, acc_active from login LEFT JOIN roles ON roles_id = id_roles where username =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", userid),

            });
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    salt = (byte[])dr["salt"];
                    key = (byte[])dr["key"];
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
                        p.SqlNonQuery("UPDATE public.login SET last_login=@d WHERE user_id = @user_id;", postgres.list = new List<NpgsqlParameter>()
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
                byte[] key = deriveBytes.GetBytes(192);  // Skapar 100-byte key av lösenordet

                return Tuple.Create(salt, key);

            }
        }
        public string GeneratePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 7; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }
        public void RegisterNewUser(string username, string email)
        {
            
            string pass = GeneratePassword();
            Users us = new Users();
            us.CreateUser(username, email, true, pass);
            Email.SendEmail(email, username, "Ditt lösenord", Email.EmailOther("Ditt lösenord är: ",pass));
        }
    }
}