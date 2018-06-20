using Matslumpiator.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Matslumpiator.Models
{
    public class Receptmodels
    {
        public int Id { get; set; }
        [Display(Name = "Namnet")]
        [Required]
        public string Name { get; set; }        
        [Display(Name = "Bild")]
        public string Url_pic { get; set; }
        [Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Url till receptet")]
        public string Url_recept { get; set; }
        [Display(Name = "Betyg")]
        public double Rating { get; set; }
        [Display(Name = "Tillägsningstid")]
        public string cookingtime { get; set; }
        [Display(Name = "Huvudprotein")]
        public string TypeOfFood { get; set; }
        [Display(Name = "Tillfälle")]
        public string Occasions { get; set; }
        public string Weeknumbers { get; set; }
        public DateTime Date { get; set; }
        public string DateName { get; set; }
        public List<Receptmodels> Recept { get; set; }
      
    }
}