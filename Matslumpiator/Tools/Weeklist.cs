using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matslumpiator.Tools
{
    public class Weeklist
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Weeklist> List()
        {
            List<Weeklist> list = new List<Weeklist>();
            for (int i = 0; i < 8; i++)
            {
                Weeklist w = new Weeklist();
                w.Id = i;
                switch (i)
                {
                    case 0:
                        w.Name = "Avstängd";
                        break;
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
                    case 6:
                        w.Name = "Lördag";
                        break;
                    case 7:
                        w.Name = "Söndag";
                        break;
                }
                list.Add(w);
            }
            
            return list;

        }
    }
}