using System;
using System.ComponentModel.DataAnnotations;

namespace Matslump.Models
{
    public class slumpmain
    {
        [Required]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
    }
}