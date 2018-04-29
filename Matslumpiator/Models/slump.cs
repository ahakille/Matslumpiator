using System.Collections.Generic;

namespace Matslumpiator.Models
{
    public class CreatSlumpView
    {

    }
    public class Slump
    {
        public List<Slump> List { get; set; }
        public string Weeknumber { get; set; }
        public List<Receptmodels> Recepts { get; set; }
       
    }
}