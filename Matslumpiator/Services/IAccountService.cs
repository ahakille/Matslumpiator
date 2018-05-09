using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matslumpiator.Services
{
    public interface IAccountService
    {
        Tuple<int, bool, string> AuthenticationUser(string ppassword, string userNameOrEmail);
        Tuple<byte[], byte[]> Generatepass(string ppassword);

        void RegisterNewUser(string username, string email, string fname, string last_name);
        bool Forgetpassword(string username);
        Tuple<int, bool, string> Resetpassword(string validate);
        void Newpassword(int login_id, string newpassword);
    }
}
