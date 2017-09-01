using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial_A_Chef.Models;

namespace Dial_A_Chef_WebApi.Models
{
    public class CusOrder
    {
        public int DishId { get; set; }
        public string CusEmail { get; set; }
        public int ChefId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsAccepted { get; set; }
        public bool SeenByChef { get; set; }
        public int Quantity { get; set; }
        public string Delivery { get; set; }
        public string PrepMethod { get; set; }

        //public string CatName { get; set; }

        public CusOrder()
        {
        }

        public CusOrder(int dishId, string cusEmail, int quantity, string delivery, string prepMethod)
        {
            if (dishId > 0) DishId = dishId;

            CusEmail = cusEmail;
            OrderDate = DateTime.Today;
            IsAccepted = false;
            SeenByChef = false;
            Quantity = quantity;
            Delivery = delivery;
            PrepMethod = prepMethod;//Home or Express

            ubDatabaseDataContext data = new ubDatabaseDataContext();
            var dish = from d in data.Dishes
                       where d.M_Id == dishId
                       select d;

            try
            {
                if (dish.FirstOrDefault() != null)
                {
                    ChefId = dish.FirstOrDefault().CHEF_Id.Value;
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

        }
        /*
            Places an order on the behalf of a customer. 
            Chef still needs to accept the order for it to be processed
            You will have to have a foreach loop to loop through each order in the basket
            Then, for every order, place the order. Get dish id from a url parameter

            Alternatively, you can put the order details in a string:
            Have an map that, Order<DishId, DishName> that will store all orders from the basket
            
        */
        public string PlaceOrder()
        {
            string strRet = "";
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            int cId = -1;
            if (FindUserId(this.CusEmail) > 0)//Find User's Id 
            {
                cId = FindUserId(this.CusEmail);
            }
            //Create a new order

            Order newOrder = new Order()
            {
                O_Date = DateTime.Today,
                Dish_ID = DishId,
                Cust_ID = cId,
                IsAccepted = false,
                SeenByChef = false,
                Chef_ID = getChefId(DishId),
                Delivery_Method = Delivery,
                Prep_Method = PrepMethod
            };
            //Save the order to the database
            data.Orders.InsertOnSubmit(newOrder);
            data.SubmitChanges();

            //Check if the order was saved to the database
            var orderExists = from d in data.Orders
                              where d.O_Id == newOrder.O_Id
                              select d;
            /*
                If the order does exist,
                save the rest of the order details to the bridging table
            */
            if (orderExists.FirstOrDefault() != null)
            {
                //Create a new DishOrderBridge object
                DishOrderBridge doBridge = new DishOrderBridge()
                {
                    Dish_Id = newOrder.Dish_ID,
                    Order_Id = newOrder.O_Id,
                    Order_Date = newOrder.O_Date,
                    Quantity = this.Quantity
                };
                //Save to the database
                data.DishOrderBridges.InsertOnSubmit(doBridge);
                data.SubmitChanges();

                /* 
                    if the details were successfully saved to the database,
                    the order was successfully processed 
                */
                var doExists = from d in data.DishOrderBridges
                               where d.DO_Id == doBridge.DO_Id
                               select d;

                if (doExists.FirstOrDefault() != null)
                {
                    strRet =
                        "Your order has been processed successfully. You will be notified if it will be accepted or rejected.";
                }
            }
            else
            {
                strRet = "There was an error while processing your order. Please try again at a later stage.";
            }

            return strRet;
        }

        /*
           Helper function that returns a user's id by querying their email address
       */
        private int FindUserId(string email)
        {
            int id = -1;
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            var userId = from d in data.Accounts
                         where d.U_Email == email
                         select d.U_ID;

            try
            {
                if (userId.First() > 0)
                {
                    id = userId.First();
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

            return id;
        }

        private int getChefId(int mealId)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var id = from i in connect.Dishes where i.M_Id == mealId select i;

            return id.First().CHEF_Id.Value;


        }

    }

}
