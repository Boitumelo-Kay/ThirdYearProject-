using Dial_A_Chef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class AccountsController : Controller
    {
        
        public ActionResult Login()
        {
            return View();
        }


        /*
         * Uses a post request for login
         */
        [HttpPost]
        public ActionResult Login(FormCollection fields)
        {
            string email = fields["email"];
            string password = fields["password"];
            UserAccount myUser = new UserAccount(email,password);
            if (myUser.Login(email, password))
            {
                JavaScriptResult result = new JavaScriptResult();
               // Response.Redirect()
            }
            else
            {
                Response.Write("Failed");
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        /*
         * Uses a post request for registering a user
         */
        [HttpPost]
        public ActionResult Register(FormCollection fields)
        {
            string name = fields["name"];
            string surname = fields["surname"];
            string email = fields["email"];
            string password = fields["password"];
            string dob = fields["dob"];
            string contact = fields["contact"];
            string image = fields["image"];
            string utype = fields["utype"];

            UserAccount myUser = new UserAccount();
            if (myUser.RegisterUser(name, surname, email, password, dob, contact, image, utype))
            {
                Response.Write("Registered");
            }
            else
            {
                Response.Write("Failed");
            }

            return View();
        }

        public ActionResult UpdateAccount()
        {
            return View();
        }

        [HttpPut]
        public ActionResult UpdateAccount(FormCollection fields)
        {
            //name surname dob contact image
            string name = fields["name"];
            string surname = fields["surname"];
            string contact = fields["contact"];
            string dob = fields["dob"];
            string image = fields["image"];
            string email = fields["email"];

            UserAccount user = new UserAccount();
            user.U_Name = name;
            user.U_Surname = surname;
            user.U_ContactNo = contact;
            user.U_Image = image;
            user.U_Email = email;

            user.UpdateUserDetails();
            return View();
        } 


    }
}