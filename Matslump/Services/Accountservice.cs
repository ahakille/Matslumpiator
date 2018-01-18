using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Matslump.Services
{
    public class Accountservice
    {
        public Tuple<int, bool, string> AuthenticationUser(string ppassword, string userNameOrEmail)
        {

            byte[] salt = null, key = null;
            int user_id = 0;
            postgres m = new postgres();
            string role = "";
            bool active;
            string sql;
            userNameOrEmail = userNameOrEmail.ToLower();
            sql = "select user_id, name, acc_active , login.salt , login.hash from users LEFT JOIN roles ON roles_id = id_roles LEFT JOIN login ON users.login_id = login.login_id where email =@par1 or users.username =@par1 ";
            var dt = m.SqlQuery(sql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", userNameOrEmail),

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
                return Tuple.Create(user_id, false, role);
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
        private string GeneratePassword(int p)
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
        public void RegisterNewUser(string username, string email, string fname, string last_name)
        {

            string pass = GeneratePassword(7);
            Users us = new Users();
            us.CreateUser(username, email, true, pass, fname, last_name);
            Email.SendEmail(email, username, "Ditt lösenord", Email.EmailOther("Ditt lösenord är: ", pass));
        }
        public bool Forgetpassword(string username)
        {
            postgres sql = new postgres();
            DataTable dt = new DataTable();
            string query;
            if (username.Contains("@"))
            {
                query = "SELECT login_id, username, email, acc_active FROM users WHERE email = @username;";
            }
            else
            {
                query = "SELECT login_id, username, email, acc_active FROM users WHERE username = @username;";
            }
            dt = sql.SqlQuery(query, postgres.list = new List<NpgsqlParameter>()
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


            if (string.IsNullOrEmpty(r.User) || !r.active)
            {
                return false;
            }
            string hash = GeneratePassword(60);
            postgres sql1 = new postgres();
            DateTime date = DateTime.UtcNow;
            date = date.AddHours(3);
            sql1.SqlNonQuery("UPDATE login set reset_hash=@hash , reset_time =@time WHERE login_id=@id  ", postgres.list = new List<NpgsqlParameter>()
                        {
                         new NpgsqlParameter("@id",r.Login_id),
                         new NpgsqlParameter("@hash",hash),
                         new NpgsqlParameter("@time",date)
                         });
            string message = Email.EmailOther(r.User, "Här kommer din återställningslänk: <a href=\"https://matslumpiator.se/Account/Resetpassword?validate=" + hash + "\" target=\"_blank\" >Återställlösenordet </a>");
            Email.SendEmail(r.email, r.User, "Återställning Lösenord", message);
            return true;

        }
        public Tuple<int, bool, string> Resetpassword(string validate)
        {
            postgres sql = new postgres();
            DataTable dt = new DataTable();
            dt = sql.SqlQuery("SELECT login.login_id, login.reset_hash, login.reset_time, users.username FROM login LEFT JOIN users ON users.login_id = login.login_id WHERE reset_hash = @hash;", postgres.list = new List<NpgsqlParameter>()
                        {
                         new NpgsqlParameter("@hash", validate)
                         });
            string hash = "";
            DateTime date = DateTime.Now;
            int login_id = 0;
            string username = "";
            foreach (DataRow dr in dt.Rows)
            {



                hash = dr["reset_hash"].ToString();
                username = dr["username"].ToString();
                date = (DateTime)dr["reset_time"];
                login_id = (int)dr["login_id"];
            }
            if (!string.IsNullOrEmpty(hash))
            {
                if (validate == hash && date >= DateTime.UtcNow)
                    return Tuple.Create(login_id, true, username);
            }

            return Tuple.Create(login_id, false, username);
        }
    }
}
