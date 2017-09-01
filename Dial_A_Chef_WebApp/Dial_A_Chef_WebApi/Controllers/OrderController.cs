using Dial_A_Chef_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class OrderController : ApiController
    {

        
        public void Post([FromBody]CusOrder order)
        {

            int dishId = order.DishId;
            string cusEmail = order.CusEmail;
            int quantity = order.Quantity;
            string delivery = order.Delivery;
            string prepMethod = order.PrepMethod;
            CusOrder meal = new CusOrder(dishId,cusEmail,quantity,delivery,prepMethod);
            meal.PlaceOrder();

//            meal.placeOrder(order.DishId,order.CusEmail,order.PrepMethod);

        }


    }
}
