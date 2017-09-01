using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using Dial_A_Chef_WebApi;
using Dail_a_chef_service.Models;
using Dial_A_Chef_WebApi.Models;
using Dial_A_Chef.Models;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            ubDatabaseDataContext meals = new ubDatabaseDataContext();

            var content = from k in meals.Dishes select k;
            List<String> Dishes = new List<String>();
            foreach (var k in content)
            {
                Dishes.Add(k.M_Name);
            }

            ViewData["Meals"] = Dishes;
            
            return View();
        }



        public ActionResult Login(FormCollection formDetails)
        {
            string email = formDetails["emailLogin"];
            string password = formDetails["passwordLogin"];
            bool exists = false;
            if (email != null)
            {
                UserAccount user = new UserAccount();

                 exists = user.Login(email, password);

                if (exists)
                {

                    Session["LogIn"] = email;
                    if (user.GetUserType(email).ToUpper().Contains("CHEF"))
                        return RedirectToAction("ChefMeals", "Chef");
                    else if (user.GetUserType(email).ToUpper().Contains("CUSTOMER"))
                        return RedirectToAction("Index", "Home");   
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
                return View();
        }

        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order(FormCollection frmDetails)
        {
            if (Session["LogIn"] != null)
            {
                string email = Session["LogIn"].ToString();
                string strId = Request.QueryString["id"];
                if (strId != null)
                {
                    //string 
                    int id = Int32.Parse((strId));
                    //CusOrder newOrder = new CusOrder(id, email, DateTime.Today, 1);
                    CusOrder newOrder = new CusOrder(id, email, 1, "home", "cash");
                    string strResult = newOrder.PlaceOrder();

                    Response.Write("<h3>"+strResult+"</h3>");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult PostWebLogin(LoginModel myUser)
        {
            LoginModel oldUser = new LoginModel();
            string strRet = "falseLogin";
            bool login = oldUser.Login(myUser.email, myUser.password);

            if (login == true)
            {

                Session["LogIn"] = myUser.email;
                strRet = "trueLogin";

                if (myUser.GetUserType(myUser.email).ToUpper().Contains("CHEF"))
                {
                    //Redirect("http://localhost:13689/Chef/ChefProfile");
                    strRet = "chefLogin";
                }
                else if (myUser.GetUserType(myUser.email).ToUpper().Contains("CUSTOMER"))
                {
                    //Redirect("http://localhost:13689/Home/Index");
                    strRet = "custLogin";
                }

            }

            return View(strRet);
        }

    }
}