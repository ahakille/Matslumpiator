using Matslumpiator.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Matslumpiator.Models
{
    public class CreatSlumpView
    {

    }
    public class slump
    {
        public List<slump> list { get; set; }
        public string Weeknumber { get; set; }
        public List<Receptmodels> recepts { get; set; }
       
    }
}