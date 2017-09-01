using Dail_a_chef_service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    public class LoginModel
    {
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }

        public LoginModel()
        {

        }
        public bool Login(string email, string password)
        {
            bool isLoggedIn = false;
            password = new StringBuilder(Hash.ComputeHash(password)).ToString();
            if (Mail.ValidateEmail(email))//validate the email address
            {
                LoginModel loginUser = new LoginModel();
                loginUser.email = email;
                if (loginUser.IsExistingUser())//check if a user with such an email exists
                {
                    //check if the existing email and password are a matching combination
                    ubDatabaseDataContext data = new ubDatabaseDataContext();
                    var exists = from d in data.Accounts
                                 where d.U_Email == email && d.U_Password == password
                                 select d;

                    if (exists.FirstOrDefault() != null)
                    {

                        isLoggedIn = true;
                    }
                    else
                    {
                        return false;
                    }
                    
                   

                }
              
            }
            return isLoggedIn;
        }
        public bool IsExistingUser()
        {
            bool exists = false;

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            try
            {
                var user = from u in data.Accounts
                           where u.U_Email == this.email
                           select u;

                if (user.FirstOrDefault() != null)
                {
                    exists = true;
                }

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

            return exists;
        }

        public string GetUserType(string email)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var user = from d in data.Accounts
                       where d.U_Email == email
                       select d;

            if (user.FirstOrDefault() != null)
            {
                Account userAcc = user.First();
                strRet = userAcc.U_Type;
            }

            return strRet;
        }




    }
}