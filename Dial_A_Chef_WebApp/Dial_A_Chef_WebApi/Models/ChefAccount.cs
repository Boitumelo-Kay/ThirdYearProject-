using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Dial_A_Chef;
using Dial_A_Chef_WebApi;

namespace Dial_A_Chef.Models
{
    public class ChefAccount
    {
        public int Ch_ID { get; set; }
        public string Ch_Type { get; set; }
        public int Ch_Rating { get; set; }
        public string Ch_Bio { get; set; }

        public ChefAccount()
        { }

        public ChefAccount(int id, string type, int rating, string bio)
        {
            Ch_ID = id;
            Ch_Type = type;
            Ch_Rating = rating;
            Ch_Bio = bio;
        }

        //Registers chef. The rating, chef bio and chef type are kept to default values
        public static bool RegisterChef(int id)
        {
            bool isRegistered = false;
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            if (!IsExistingChef(id))
            {
                Chef newChef = new Chef()
                {
                    Ch_ID = id,
                    Ch_Bio = "",
                    Ch_Rating = 0,
                    Ch_Type = ""
                };

                data.Chefs.InsertOnSubmit(newChef);
                data.SubmitChanges(ConflictMode.FailOnFirstConflict);

                if (IsExistingChef(newChef.Ch_ID))
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        }

        //helper function to check if the chef exists in the database
        private static bool IsExistingChef(int id)
        {
            bool isExists = false;
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var exists = from c in data.Chefs
                         where c.Ch_ID == id
                         select c;
            try
            {
                if (exists.FirstOrDefault() != null)
                {
                    isExists = true;
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException();
            }

            return isExists;
        }
        /*
            Updates the chef's bio and chef type
            The chef rating should be calculated automatically
        
        */
        public void UpdateChefDetails()
        {
            ubDatabaseDataContext data = new ubDatabaseDataContext();

            var chef = data.Accounts.FirstOrDefault(d => d.U_ID == this.Ch_ID);

            if (chef != null)
            {
                chef.Chef.Ch_Type = this.Ch_Type;
                chef.Chef.Ch_Bio = this.Ch_Bio;

                data.SubmitChanges();
            }
        }

    }
}