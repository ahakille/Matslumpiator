﻿using Matslumpiator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Matslumpiator.Services
{
    public class Slumpcron: ISlumpCronService
    {
        private readonly IEmailService _emailService;
        private readonly IUserServices _userServices;
        private readonly ISlumpServices _slumpServices;

        public Slumpcron(IEmailService emailService, IUserServices userServices,ISlumpServices slumpServices)
        {
            _emailService = emailService;
            _userServices = userServices;
            _slumpServices = slumpServices;

        }
        public void StartCron()
        {
            
          //  Slumpservices checkslump = new Slumpservices();
            DateTime crondate = DateTime.Now;
            int checkcron = ((int)crondate.DayOfWeek == 0) ? 7 : (int)crondate.DayOfWeek;
            List<UserService> list = _userServices.GetuserAsAdmin(checkcron, "SELECT users.user_id, users.username, users.fname, users.last_name, users.email, users.acc_active,users.roles_id,users.settings_id,users.last_login FROM public.users LEFT JOIN usersettings ON users.settings_id = usersettings.setting_id  WHERE usersettings.day_of_slumpcron =@id ;");
           
            foreach (var item in list)
            {
                DateTime date = _slumpServices.datefixer(DateTime.Now);
                date = date.Date;
                date = date.AddDays(7);
                if(item.active)
                {
                    bool check = _slumpServices.Checkslump(date, item.User_id);
                    if (check)
                    {

                    }
                    else
                    {

                       // var slumpa = new Slumpservices();
                        List<Receptmodels> lista = _slumpServices.Slumplist(item.User_id, date);
                        if (lista[0].Id != -10)
                        {
                            string body = EmailService.Emailslumplist(item.First_name, "Här kommer nästa veckas mat. Hoppas de ska smaka!", lista);
                            
                            _emailService.SendEmail(item.email, item.First_name, "Här kommer nästa veckas mat.", body);
                            foreach (var items in lista)
                            {

                                _slumpServices.SaveSlump(items.Id, item.User_id, items.Date, false);
                            }


                        }
                        else
                        {
                          //  var getlist = new Slumpservices();
                            var slumplist = _slumpServices.CreateRandomListOfRecept();
                            string body = EmailService.EmailRadomlist(item.User, "Tyvärr finns det inte tillräckligt med maträtter i din personliga lista. </br> Går gärna in och lägg till de rätter som passar dig så kan vi hjälpa dig med förslag till middag.<br> Vi skickade med några förslag", slumplist, item.User_id);

                            _emailService.SendEmail(item.email, item.User, "Vi behöver din hjälp", body);
                        }


                    }
                }
                
            }
        }
    }
}