using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Matslumpiator.Tools
{
    public class Weeklist
    {
        public string Name { get; set; }

        public static List<string> List()
        {
            List<string> list = new List<string>();
            string Name="";
            for (int i = 0; i < 8; i++)
            {
                      
                switch (i)
                {
                    case 0:
                        Name = "Avstängd";
                        break;
                    case 1:
                        Name = "Måndag";
                        break;
                    case 2:
                        Name = "Tisdag";
                        break;
                    case 3:
                        Name = "Onsdag";
                        break;
                    case 4:
                        Name = "Torsdag";
                        break;
                    case 5:
                        Name = "Fredag";
                        break;
                    case 6:
                        Name = "Lördag";
                        break;
                    case 7:
                        Name = "Söndag";
                        break;
                }
                list.Add(Name);
            }
            
            return list;

        }
        public static int CheckCronoDay(string Day)
        {
            int numberOfday = 0;
            switch (Day)
            {
                case "Avstängd":
                    numberOfday = 0;
                    break;
                case "Måndag":
                    numberOfday = 1;
                    break;
                case "Tisdag":
                    numberOfday = 2;
                    break;
                case "Onsdag":
                    numberOfday = 3;
                    break;
                case "Torsdag":
                    numberOfday = 4;
                    break;
                case "Fredag":
                    numberOfday = 5;
                    break;
                case "Lördag":
                    numberOfday = 6;
                    break;
                case "Söndag":
                    numberOfday = 7;
                    break;
            }
            return numberOfday;
        }
        public static string CheckCronoNumber(int i)
        {
            string Name ="";

           
            switch (i)
            {
                
                case 0:
                    Name = "Avstängd";
                    break;
                case 1:
                    Name = "Måndag";
                    break;
                case 2:
                    Name = "Tisdag";
                    break;
                case 3:
                    Name = "Onsdag";
                    break;
                case 4:
                    Name = "Torsdag";
                    break;
                case 5:
                    Name = "Fredag";
                    break;
                case 6:
                    Name = "Lördag";
                    break;
                case 7:
                    Name = "Söndag";
                    break;
            }
                
            

            return Name;

        }
    }
}