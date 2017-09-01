using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    /*
        Class that stores an individual cart item
    */
    public class BasketItem
    {
        public string ItemName { get; set; }

        public string Description { get; set; }

        public int PointValue { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

        public int TotalPoints { get; set; }


        public BasketItem()
        {
            TotalPoints = 0;
        }

        public BasketItem(string name, string descr, int points, string cat, int quant)
        {
            ItemName = name;
            Description = descr;
            PointValue = points;
            Category = cat;
            Quantity = quant;
        }

        public int CalcTotalItemPoints()
        {
            TotalPoints = 0;

            if (this != null)
            {
                TotalPoints = PointValue * Quantity;
            }

            return TotalPoints;
        }

    }
}