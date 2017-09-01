using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial_A_Chef;
using Dial_A_Chef_WebApi;

namespace Dial_A_Chef.Models
{
    public class CustomerAccount
    {
        public int Cu_ID { get; set; }
        public string Cu_DietaryR { get; set; }
        public int Cu_Points { get; set; }

        public CustomerAccount()
        { }

        public CustomerAccount(int id, string dietaryR, int points)
        {
            Cu_ID = id;
            Cu_DietaryR = dietaryR;
            Cu_Points = points;
        }

        private static bool IsExistingCust(int id)
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            bool isFound = false;
            
            var exists = from c in data.Customers
                           where c.Cu_ID == id
                           select c;
            try
            {
                if (exists.First() != null)
                {
                    isFound = true;
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }
            return isFound;
        }

        public static bool RegisterCust(int id)
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();
            bool isRegistered = false;
            if (!IsExistingCust(id))
            {
                Customer newCust = new Customer()
                {
                    Cu_ID = id,
                    Cu_DietaryR = "",
                    Cu_Points = 0
                };

                data.Customers.InsertOnSubmit(newCust);
                data.SubmitChanges();

                if (IsExistingCust(newCust.Cu_ID))
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        }

        /*
            Updates the Customers dietary requirements and customer points only
        */
        public void UpdateCustomer()
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var customer = from d in data.Accounts
                where d.U_ID == this.Cu_ID
                select d;

            try
            {
                if (customer.First() != null)
                {
                    customer.First().Customer.Cu_Points = this.Cu_Points;
                    customer.First().Customer.Cu_DietaryR = this.Cu_DietaryR;

                    data.SubmitChanges();
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

        }
        
    }
}