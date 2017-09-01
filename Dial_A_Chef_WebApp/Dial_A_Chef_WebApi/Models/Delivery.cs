using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    public class Delivery
    {
        public int OrderId { get; set; }
        public bool DeliveryStatus { get; set; }

        public int DelTime { get; set; } /// <summary>
        /// time delay for the delivery
        /// </summary>
        public DateTime DelExpected { get; set; }/// <summary>
        /// Calculated: Today.Time + DelTime
        /// </summary>
        public Delivery()
        {
            DeliveryStatus = false;
        }

        public Delivery(int oId,int TimeDelay)
        {
            OrderId = oId;
            DelTime = TimeDelay;
            DeliveryStatus = false;
        }

        public string StartDelivery()
        {
            DateTime today = DateTime.Today;
            string strRet = "";
            DelExpected = today.AddMinutes(Double.Parse(DelTime.ToString()));

            Dial_A_Chef_WebApi.Delivery del = this.InsertDelivery();

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            if(del != null)
            {
                //Dial_A_Chef_WebApi.Delivery del = delivery.FirstOrDefault();

                return "Your Delivery should arrive around " + DelExpected.TimeOfDay.ToString(); 
            }
            else
            {
                return strRet;
            }
        }

        public bool ConfirmDelivery()
        {
           // bool isDelivered = false;

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var del = from d in data.Deliveries
                      where d.Order_Id == this.OrderId
                      select d;

            if(del.FirstOrDefault()!=null)
            {
                Dial_A_Chef_WebApi.Delivery delivery = del.FirstOrDefault();
                delivery.Del_Status = true;
                data.SubmitChanges();

                return true;

            }
            else
            {
                return false;
            }
        }

        public Dial_A_Chef_WebApi.Delivery InsertDelivery()
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            Dial_A_Chef_WebApi.Delivery delivery = new Dial_A_Chef_WebApi.Delivery()
            {
                Order_Id = this.OrderId,
                Del_Status = this.DeliveryStatus,
                Del_Time = this.DelTime,
                Del_ExpectedTime = this.DelExpected
            };

            data.Deliveries.InsertOnSubmit(delivery);
            data.SubmitChanges();

            var exists = from d in data.Deliveries
                         where d.Del_Id == delivery.Del_Id
                         select d;

            if(exists.FirstOrDefault()!=null)
            {
                return exists.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public bool QueryDelivery()
        {
            bool isDelivered = false;
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var del = from d in data.Deliveries
                      where d.Order_Id == OrderId
                      select d;

            if (del.FirstOrDefault()!=null)
            {
               isDelivered = del.FirstOrDefault().Del_Status.Value;
            }

            return isDelivered;
        }

    }
}