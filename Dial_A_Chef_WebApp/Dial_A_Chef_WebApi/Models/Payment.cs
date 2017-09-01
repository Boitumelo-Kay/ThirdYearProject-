using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    public class Payment
    {
        public int OrderId { get; set; }
        public int TotalPoiints { get; set; }

        public Payment()
        {
        }

        public Payment(int oId )
        {
            OrderId = oId;
        }

        /*
            Processes the total payment of an order
            For multiple orders, use a loop
            (Use) Url Parameter to get order id
        */
        public bool ProcessPayment()
        {
            bool isProcessed = false;
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            
            OrderPayDetail opDet = new OrderPayDetail()
            {
                Order_Id = OrderId
            };

            //Fetch the actual order from the database
            var orderData = from o in data.Orders
                where o.O_Id == OrderId
                select o;

            Order order = null;

            if (orderData.First() != null)
            {
                order = (Order) orderData.First();
                var chefData = from c in data.Chefs
                    where c.Ch_ID == order.Chef_ID
                    select c;
                //Query Customer details according to id in order
                //will have to minus points when processing payment
                var custData = from cu in data.Customers
                    where cu.Cu_ID == order.Cust_ID
                    select cu;
                //Query Dish details according to id in order
                //will give dish price
                var dishData = from d in data.Dishes
                    where d.M_Id == order.Dish_ID
                    select d;
                //Query Chef details according to id in order
                //add points to chef to process payment
                if (chefData.First() != null && custData.First() != null && dishData.First()!=null)
                {
                    Chef chef = chefData.First();
                    Customer cust = custData.First();
                    Dish dish = dishData.First();

                    if (order.Delivery_Method.ToLower() == "home")
                    {
                        opDet.Total_Points = dish.M_Points + 20;//extra charge on home prep
                    }
                    else
                    {
                        opDet.Total_Points = dish.M_Points;
                    }

                }

            }
            return isProcessed;
        }
       
    }
}