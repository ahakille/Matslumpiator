using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matslump.Tools
{
    public class Weeklist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Weeklist> List()
        {
            List<Weeklist> list = new List<Weeklist>();
            for (int i = 1; i < 6; i++)
            {
                Weeklist w = new Weeklist();
                w.Id = i;
                switch (i)
                {
                    case 1:
                        w.Name = "Måndag";
                        break;
                    case 2:
                        w.Name = "Tisdag";
                        break;
                    case 3:
                        w.Name = "Onsdag";
                        break;
                    case 4:
                        w.Name = "Torsdag";
                        break;
                    case 5:
                        w.Name = "Fredag";
                        break;
                }
                list.Add(w);
            }
            
            return list;

        }
    }
}