using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHK_INCHK_OUT.Model
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }

      
        public Login(string name, string pasword)
        {
            this.Username = name;
            this.Password = pasword;
            
        }
    }
}
