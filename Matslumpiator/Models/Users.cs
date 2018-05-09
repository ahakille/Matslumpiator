using Matslumpiator.Services;
using Matslumpiator.Tools;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace Matslumpiator.Models
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
        public string CronoDay { get; set; }
    }
    public class UserService
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


    }
}