using Matslump.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Matslump.Models
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
        [Display(Name = "Tillagsningstid")]
        public string cookingtime { get; set; }
        [Display(Name = "Huvudprotein")]
        public string TypeOfFood { get; set; }
        [Display(Name = "Tillfälle")]
        public string Occasions { get; set; }
        public string Weeknumbers { get; set; }
        public DateTime Date { get; set; }
        public List<Receptmodels> Recept { get; set; }

    }
    public class ReceptmodelsAddEditView
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
        [Display(Name = "Tillagsningstid")]
        public int CookingtimeId { get; set; }
        [Display(Name = "Huvudprotein")]
        public int TypeOfFoodId { get; set; }
        [Display(Name = "Tillfälle")]
        public int Occasions { get; set; }
        [Display(Name = "Icas Id")]
        public int Ica_id { get; set; }

    }
}