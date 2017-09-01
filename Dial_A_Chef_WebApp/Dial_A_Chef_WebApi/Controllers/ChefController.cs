using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Dail_a_chef_service.Models;
using Dial_A_Chef_WebApi;
using Dail_a_chef_service.Model;
using Dial_A_Chef.Models;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class ChefController : Controller, IController
    {
        private string oldMeal;

        /**
         * @Returns meals for a particular chef 
         */
        public ActionResult ChefMeals()
        {
            
            string mail = Request.QueryString["email"].ToString();

            Session["LogIn"] = mail;
            Session.Timeout = 10;
            string email = (string)Session["LogIn"];

            if (mail !=null && UserAccount.GetType(mail).ToLower().Contains("chef"))
            {
                CHEF chefAccount = new CHEF(email);

                List<Dish> chefMeals = chefAccount.getDishes(getId(mail));

                List<string> meals = new List<string>();
                List<string> ingredients = new List<string>();
                List<string> description = new List<string>();
                List<int> price = new List<int>();
                for (int i = 0; i < chefMeals.Count; i++)
                {
                    meals.Add(chefMeals[i].M_Name);
                    ingredients.Add(chefMeals[i].M_Ingridients);
                    description.Add(chefMeals[i].M_Description);
                    price.Add(chefMeals[i].M_Points);
                }


                ViewData["Meals"] = meals;
                ViewData["Ingredients"] = ingredients;
                ViewData["Description"] = description;
                ViewData["Price"] = price;
            }
            
            return View();
        }


        public ActionResult EditMeal()
        {

            string email =(string) Session["LogIn"];
            CHEF chef = new CHEF(email);

            oldMeal = Request.QueryString["name"];
            Session["oldMeal"] = oldMeal;
            ViewData["Name"] = oldMeal;
            ViewData["Ingredients"] = chef.getDishIngredients(oldMeal);
            ViewData["Price"] = chef.getDishPrice(oldMeal);
            ViewData["Description"] = chef.getDishDescription(oldMeal);

            return View();
        }

        

        [HttpPost]
        public ActionResult EditMeal(HttpPostedFileBase file,FormCollection formDetails)
        {
            string email =(string) Session["LogIn"];
            CHEF chef = new CHEF(email);
            string path = null;

            string oldMeal = (string)Session["oldMeal"];
            if (file != null)
            {

                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    path = Path.Combine(Server.MapPath("~/App_Data/Uploads/"), fileName);
                    file.SaveAs(path);

                    chef.updateMeal(oldMeal, formDetails["mName"].ToString(), formDetails["mPrice"].ToString(), formDetails["mIngred"].ToString(), formDetails["mDesc"].ToString(), path);


                }
                else
                {
                    string imagePath = getImagePath(chef.getMealId(email, oldMeal));
                    chef.updateMeal(oldMeal, formDetails["mName"].ToString(), formDetails["mPrice"].ToString(), formDetails["mIngred"].ToString(), formDetails["mDesc"].ToString(), imagePath);

                }

            }
            return RedirectToAction("ChefMeals","Chef");
        }


        public ActionResult DeleteMeal()
        {
            string email = (string)Session["LogIn"];
            CHEF chef = new CHEF(email);

            string meal = Request.QueryString["name"]; 
            chef.DeleteMeal(meal);

            return RedirectToAction("ChefMeals", "Chef");
        }

        public ActionResult AddMeal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMeal(HttpPostedFileBase file, FormCollection formData)
        {
            string email = (string)Session["LogIn"];
            
            CHEF chef = new CHEF(email);

            string mealName = formData["mName"];
            int price = Int32.Parse(formData["mPrice"]);
            string description = formData["mDesc"];
            string ingredients = formData["mIngred"];
            string category = formData["category"];
            string image = formData["file"];
            string path = null;


            if (file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/Uploads/"), fileName);
                file.SaveAs(path);
            }


           chef.upload(mealName, ingredients, description, path, price, category);


            List<Dish> chefMeals = chef.getDishes(getId(email));

            List<string> meals = new List<string>();
            List<string> ingredientsList = new List<string>();
            List<string> descriptionList = new List<string>();
            List<int> priceList = new List<int>();
            for (int i = 0; i < chefMeals.Count; i++)
            {
                meals.Add(chefMeals[i].M_Name);
                ingredientsList.Add(chefMeals[i].M_Ingridients);
                descriptionList.Add(chefMeals[i].M_Description);
                priceList.Add(chefMeals[i].M_Points);
            }


            ViewData["Meals"] = meals;
            ViewData["Ingredients"] = ingredientsList;
            ViewData["Description"] = descriptionList;
            ViewData["Price"] = priceList;


            return RedirectToAction("ChefMeals", "Chef");
        }

        public ActionResult ChefProfile()
        {
            string mail = Request.QueryString["email"].ToString();
            if(mail==null)
            {
                throw new NullReferenceException();
            }
            return View();
        }
        
        
        public ActionResult ProcessOrders()
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            //string mail = Request.QueryString["email"].ToString();
            //Session["LogIn"] = mail;
            string mail = (string)Session["LogIn"];
            if (IsAChef(mail)==true)
            {
                var orders = from o in data.Orders
                             where o.Chef.Account.U_Email == mail
                             select o;

                try
                {

                    if (orders.FirstOrDefault() != null)
                    {
                        List<Order> orderList = new List<Order>();
                        orderList = orders.ToList();

                        List<Order> oListP = new List<Order>();
                        foreach(var o in orderList)
                        {
                            if(o.Processed == false||o.Processed == null)
                            {
                                oListP.Add(o); 
                                
                            }
                        }

                        ViewData["TotalOrders"] = orders.Count();//num total orders
                        ViewData["pOrders"] = orders.Count() - oListP.Count();//num processed orders
                        ViewData["OrderList"] = orderList;//all of the orders
                        ViewData["oList"] = oListP;
                    }
                }
                catch(NullReferenceException)
                {

                }


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /*public ActionResult ChefProfile(FormCollection fields)
        {
            return View();
        }
        */

        private string getImagePath(int mealId)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var image = from i in connect.Dishes where i.M_Id == mealId select i.M_MealImage;

            return image.First();

        }
       
        private int getId(string email)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var id = (from i in connect.Accounts where i.U_Email == email select i.U_ID);

            return id.First();
        }

        public bool IsAChef(string email)
        {
            bool isChef = false;
            if(Mail.ValidateEmail(email))
            {
                if(UserAccount.GetType(email).ToLower().Contains("chef"))
                {
                    isChef = true; ;
                }
            }
            else
            {
                return false;
            }
            return isChef;
        }

        public ActionResult OrderAccept(int id)
        {
            AcceptOrder(id);
            return RedirectToAction("ProcessOrders", "Chef");
        }

        /*
        public ActionResult OrderReject(int id, string reason)
        {
            RejectOrder(id, reason);
            return RedirectToAction("ProcessOrders", "Chef");
        }
        */

        public string AcceptOrder(int orderId)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var order = from o in data.Orders
                        where o.O_Id == orderId
                        select o;

            if (order.FirstOrDefault() != null)
            {
                Order existOrder = order.FirstOrDefault();
                existOrder.SeenByChef = true;
                existOrder.IsAccepted = true;
                existOrder.Processed = true;

                strRet = "Order has been processed";
                
                data.SubmitChanges();

            }
            else
            {
                return "";
            }
            return strRet;
        }

        /*
        public string RejectOrder(int orderId, string reason)
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var order = from o in data.Orders
                        where o.O_Id == orderId && o.SeenByChef == false
                        select o;

            if (order.FirstOrDefault() != null)
            {
                Order existOrder = order.FirstOrDefault();
                existOrder.SeenByChef = true;
                existOrder.IsAccepted = false;

                strRet = "Your Order has been Rejected";

                data.SubmitChanges();

            }
            else
            {
                return "";
            }
            return strRet;
        }
        */
    }
}