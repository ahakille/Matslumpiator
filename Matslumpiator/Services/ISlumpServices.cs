using Matslumpiator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public interface ISlumpServices
    {
        List<Receptmodels> CreateRandomListOfRecept();
        List<Receptmodels> Slumplist(int user_id, DateTime date);
        DateTime datefixer(DateTime date);
        int GetIso8601WeekOfYear(DateTime time);
        bool Checkslump(DateTime date, int user_id);
        void SaveSlump(int recept_id, int user_id, DateTime date, bool check);
        List<Receptmodels> Oldslumps(int user_id, DateTime date, DateTime dateto);
        List<Slump> Weeknumbers(List<Receptmodels> lista);
    }
}
