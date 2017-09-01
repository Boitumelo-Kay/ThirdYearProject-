using Dial_A_Chef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class MobileController : ApiController
    {
        public bool PostLogin([FromBody] UserAccount oldUser)
        {
            String username = oldUser.U_Email;
            String password = oldUser.U_Password;

            UserAccount myUser = new UserAccount(username, password);

            bool login = myUser.Login(myUser.U_Email, myUser.U_Password);

         
            return login;
        }

        public bool PostRegister([FromBody] UserAccount newUser)
        {

            String username = newUser.U_Email;
            String password = newUser.U_Password;
            String name = newUser.U_Name;
            String surname = newUser.U_Surname;
            String userType = "Customer";
            String contact = newUser.U_ContactNo;
            String dob = newUser.U_DOB;

            UserAccount myUser = new UserAccount(name, surname, username, password, dob, contact, null, userType);
            return myUser.RegisterUser();

        }

    } 
}
