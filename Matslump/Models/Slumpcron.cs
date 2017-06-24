using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Matslump.Models
{
    public class Slumpcron
    {
     public static void StartCron()
        {
            Users user = new Users();
            List<Users> list = user.Getuser(6, "SELECT login.user_id, login.username, login.roles_id, login.email, login.acc_active, login.last_login, login.day_of_slumpcron FROM public.login WHERE login.day_of_slumpcron =@id ;");
            slump checkslump = new slump();
            foreach (var item in list)
            {
                DateTime date = checkslump.datefixer(DateTime.Now);
                date = date.Date;
                date = date.AddDays(7);
                bool check = checkslump.Checkslump(date, item.User_id);
                if (check)
                {
                    //Email.SendEmail(item.email, item.User, "test", "test" + DateTime.Now);
                }
                else
                {
                    
                    slump slumpa = new slump();
                    List<Receptmodels> lista = slumpa.Slumplist(item.User_id, date);
                    if(lista[0].Id != 10)
                    {
                        string body = Email.Emailslumplist(item.User, "Här kommer nästa veckas mat. Hoppas de ska smaka!", lista);
                        Email.SendEmail(item.email, item.User, "Här kommer nästa veckas mat.", body);
                        foreach (var items in lista)
                        {

                            slumpa.SaveSlump(items.Id, item.User_id, items.Date, false);
                        }
                        

                    }
                    else
                    {
                        string body = Email.EmailOther(item.User, "Tyvärr finns det inte tillräckligt med maträtter i din personliga lista. </br> Går gärna in och lägga till de rätter som passar dig så kan vi hjälpa dig med förslag till middag.");
                        Email.SendEmail(item.email, item.User, "Vi behöver din hjälp", body);
                    }
                   
                   
                }
            }
        }
    }
}