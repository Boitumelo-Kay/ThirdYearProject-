using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dail_a_chef_service.Models;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Models
{
    public class CUSTOMER : USER
    {

        private string dietryRequirements;
        private int points;
        private int ID;

        public CUSTOMER()
        {
            dietryRequirements = null;
            points = 0;
           
        }

        public CUSTOMER(string diet, int points)
        {
            dietryRequirements = diet;
            this.points = points;
        }




        public bool registerCus()
        {
            bool cusReg = false;
            Customer newCustomer = new Customer()
            {
                Cu_ID = getID(),
                Cu_DietaryR = getDiet(),
                Cu_Points = getPoints()
            };
          
            ubDatabaseDataContext content = new ubDatabaseDataContext();

            content.Customers.InsertOnSubmit(newCustomer);
            content.SubmitChanges();
            cusReg = true;


            return cusReg;

        }


        //Accessors and Mutators

        public void setPoints(int p)
        {
            points = p;
        }

        public int getPoints()
        {
            return points;
        }

        public void setDiet(string diet)
        {
            dietryRequirements = diet;
        }

        public string getDiet()
        {
            return dietryRequirements;
        }

        public void setID(int ID)
        {
            this.ID = ID;

        }

        public int getID()
        {
            return ID;
        }
    }
}