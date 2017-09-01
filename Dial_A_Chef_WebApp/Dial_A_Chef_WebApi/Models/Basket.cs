using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    /*
        Class that will hold basket items and their id from the database;
    */
    public class Basket
    {
        public int Total { get; set; }

        public Dictionary<int,BasketItem> BasketItems{get;set;}//will get the item's id from database

        public Basket()
        {
            BasketItems = new Dictionary<int, BasketItem>();
            Total = 0;
        }

        /*
            Uses the Dictionary to calculate the total
            The key is the price, the value is the quantity, or vica versa
        */
        public int CalcTotal()
        {
            int total = -1;

            if (BasketItems != null)
            {
                total = 0;
                foreach(var dish in BasketItems)
                {
                    total += dish.Value.TotalPoints;
                }
            }
            Total = total;
            if(Total>-1)
            {
                return Total;
            }
            return total;
        }
    }
}