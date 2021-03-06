﻿using System.ComponentModel.DataAnnotations;
namespace Matslumpiator.Models
{
    public class Accountmodels
    {
        [Required]
        [Display(Name = "Användarnamn eller Email")]
        // [EmailAddress]
        public string User { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
    public class CreateAccountViewmodel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} måste minst vara {2} tecken långt.", MinimumLength = 6)]
        [Display(Name = "Användarnamn*")]
        public string User { get; set; }
        [Required]
        [Display(Name = "Förnamn*")]
        public string First_name { get; set; }
        [Required]
        [Display(Name = "Efternamn*")]
        public string Last_name { get; set; }
        [Required]
        [Display(Name = "E-mail*")]
        [EmailAddress]
        public string email { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "Du måste godkänna användarvilkoren")]
        public bool UserAgreement { get; set; }

    }
    public class NewPasswordViewmodel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Verifiera lösenord")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}