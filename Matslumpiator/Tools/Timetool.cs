using Matslumpiator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Matslumpiator.Tools
{
    public class Timetool
    {
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string TranslateDayOfWeek(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "Monday":
                    dayOfWeek = "Måndag";
                    break;
                case "Tuesday":
                    dayOfWeek = "Tisdag";
                    break;
                case "Wednesday":
                    dayOfWeek = "Onsdag";
                    break;
                case "Thursday":
                    dayOfWeek = "Torsdag";
                    break;
                case "Friday":
                    dayOfWeek = "Fredag";
                    break;
            }
                    return dayOfWeek;
        }
        public List<Slump> Weeknumbers(List<Receptmodels> lista)
        {

            List<Slump> list = new List<Slump>();
            string check = "";
            foreach (var item in lista)
            {
                if (check != item.Weeknumbers)
                {
                    Slump sl = new Slump();
                    sl.Weeknumber = item.Weeknumbers;
                    list.Add(sl);
                    check = item.Weeknumbers;
                }

            }
            return list;

        }
    }
}