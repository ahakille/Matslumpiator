using Matslumpiator.Models;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

namespace Matslumpiator.Services
{
    public class Accountservice : IAccountService
    {
        //private readonly IUserServices _userServices;
        private readonly IEmailService _emailService;

        public Accountservice( IEmailService emailService)
        {
            _emailService = emailService;
        }
        public Tuple<int, bool, string> AuthenticationUser(string ppassword, string userNameOrEmail)
        {

            byte[] salt = null, key = null;
            int user_id = 0;
            postgres m = new postgres();
            string role = "";
            bool active = false;
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

            if (salt != null && active == true)
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
           
            CreateUser(username, email, true, pass, fname, last_name);
          
            _emailService.SendEmail(email, username, "Ditt lösenord", EmailService.EmailOther("Ditt lösenord är: ", pass));
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
            UserService r = new UserService();
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
            string message = EmailService.EmailOther(r.User, "Här kommer din återställningslänk: <a href=\"https://matslumpiator.se/Account/Resetpassword?validate=" + hash + "\" target=\"_blank\" >Återställlösenordet </a>");
            
            _emailService.SendEmail(r.email, r.User, "Återställning Lösenord", message);
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


        public void Newpassword(int login_id, string newpassword)
        {

            Tuple<byte[], byte[]> password = Generatepass(newpassword);
            postgres sql = new postgres();
            // behöver skrivas om! klart
            sql.SqlNonQuery("UPDATE login set salt= @par2, hash =@par3 WHERE login_id =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", login_id),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });
        }
        private void CreateUser(string user, string email, bool active, string Password, string fname, string last_name)
        {

            Tuple<byte[], byte[]> password = Generatepass(Password);
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
    }
}
