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
        public class NewpasswordViewModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
            [Display(Name = "Användarnamn")]
            public string User { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        

        public Tuple<int,bool,string> AuthenticationUser(string ppassword, string userid)
        {

            byte[] salt = null, key = null;
            int user_id =0;
            postgres m = new postgres();
            string role= "";

           var dt = m.SqlQuery("select salt, key, user_id, name from login LEFT JOIN roles ON roles_id = id_roles where username =@par1", postgres.list = new List<NpgsqlParameter>()
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
    }
}