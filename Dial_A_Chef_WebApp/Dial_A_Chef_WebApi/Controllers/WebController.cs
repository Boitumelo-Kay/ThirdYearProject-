using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dial_A_Chef.Models;
using Dial_A_Chef_WebApi.Models;
using System.Web;
using System.Web.SessionState;

namespace Dial_A_Chef.Controllers
{
    public class WebController : ApiController
    {
        // GET api/values
        public IEnumerable<UserAccount> Get()
        {
            return UserAccount.UsersList();
        }

        // GET api/values/5
        public UserAccount Get(int id)
        {
            return UserAccount.FindUserAccount(id);
        }
        // PUT api/values/5
        /*  public void Put([FromBody]UserAccount account)
          {
              account?.UpdateUserDetails();
          }*/

        // DELETE api/values/5
        public String PostWebLogin([FromBody]LoginModel myUser)
        {
            LoginModel oldUser = new LoginModel();
            string strRet = "falseLogin";
            bool login = oldUser.Login(myUser.email, myUser.password);

            if (login == true)
            {
                //HttpContext.Current.Session.
               // HttpContext.Current.Session.Add("LogIn", myUser.email);
                strRet = "trueLogin";

                if (myUser.GetUserType(myUser.email).ToUpper().Contains("CHEF"))
                {
                    //Redirect("http://localhost:13689/Chef/ChefProfile/");
                    strRet = "chefLogin";
                }
                else if (myUser.GetUserType(myUser.email).ToUpper().Contains("CUSTOMER"))
                {
                    //Redirect("http://localhost:13689/Home/Index");
                    strRet = "custLogin";
                }

            }


           

            return strRet;
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

