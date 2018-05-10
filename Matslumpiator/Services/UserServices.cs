using Matslumpiator.Models;
using Matslumpiator.Tools;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public class UserServices : IUserServices
    {
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;

        public UserServices(IAccountService accountServices, IEmailService emailService)
        {
            _accountService = accountServices;
            _emailService = emailService;


        }
        public List<UserService> GetuserAsAdmin(int id, string sql)
        {
            postgres m = new postgres();
            DataTable dt = new DataTable();
            List<UserService> mt = new List<UserService>();
            dt = m.SqlQuery(sql, postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@id", id)
            });
            foreach (DataRow dr in dt.Rows)
            {

                UserService r = new UserService();
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
            System.Data.DataTable dt = new DataTable();
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
                r.CronoDay = Weeklist.CheckCronoNumber((int)dr["day_of_slumpcron"]);
                mt.Add(r);
            }

            return mt;
        }
        
        public void UpdateUser(int User_id, string username, string email, string first_name, string last_name, int Slumpday)
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
        public void Newpassword(int login_id, string newpassword)
        {
          
            Tuple<byte[], byte[]> password = _accountService.Generatepass(newpassword);
            postgres sql = new postgres();
            // behöver skrivas om! klart
            sql.SqlNonQuery("UPDATE login set salt= @par2, hash =@par3 WHERE login_id =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", login_id),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });
        }
        public async Task SendMessagesToAllUsers(string subject, string message)
        {
            List<UserService> list = GetuserAsAdmin(0, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id ,users.last_login,users.settings_id FROM public.users");
            foreach (var item in list)
            {
                
                
                await _emailService.SendEmail(item.email, item.User, subject, EmailService.EmailOther(item.First_name + " " + item.Last_name, message));
                
                   

            }
           

        }
        public void DeleteUser(int User_id)
        {
            postgres sql = new postgres();
            sql.SqlNonQuery("UPDATE Users SET username=@user, email=@email, acc_active = false WHERE user_id = @user_id", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@user_id", User_id),
                new NpgsqlParameter("@user","test"),
                new NpgsqlParameter("@email","mat@nppc.se")


            });
        }
    }
}
