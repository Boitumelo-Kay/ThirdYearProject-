using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dail_a_chef_service.Models;
using Dial_A_Chef_WebApi;

namespace Dail_a_chef_service.Model
{
    public class CHEF : USER
    {
        private string type;
        private string bio;
        private int rating;
        private int id;



        public CHEF(string email)
        {
            setEmail(email);
            this.type = null;
            this.bio = null;
            this.rating = 0;
        }

        public CHEF(string email,string type, string bio, int rating)
        {
            setEmail(email);
            this.type = type;
            this.bio = bio;
            this.rating = rating;

        }



        public bool registerChef()
        {
            bool registered = false;
            Chef newChef = new Chef()
            {
                Ch_ID = getID(),
                Ch_Type = getType(),
                Ch_Rating = getRating(),
                Ch_Bio = getBio(),
            };

       

            ubDatabaseDataContext content = new ubDatabaseDataContext();

            content.Chefs.InsertOnSubmit(newChef);
            content.SubmitChanges();
            registered = true;

            return registered;

        }




        /**
        *A method that returns a list of all registered chefs in the database 
         */
        public List<Account> getChefs()
        {

            ubDatabaseDataContext content = new ubDatabaseDataContext();

            var chefs = from k in content.Accounts where k.U_Type == "chef" select k;

            List<Account> data = new List<Account>();

            foreach (var k in chefs)
            {
                data.Add(k);
            }

            return data;
        }


        /**
         * A method that returns a list of meals registered under a chef
        */
        public List<Dish> getDishes(int chefID)
        {
            ubDatabaseDataContext content = new ubDatabaseDataContext();

            var meals = from t in content.Dishes
                        where t.CHEF_Id == chefID
                        select t;

            List<Dish> dishes = new List<Dish>();

            foreach (Dish m in meals)
            {
                dishes.Add(m);
            }

            return dishes;
        }



        public string getDishIngredients(string dishName)
        {

            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var ingredients = from c in connect.Dishes where c.M_Name == dishName && c.CHEF_Id == getID(getEmail()) select c.M_Ingridients;

            return ingredients.First();
        }


        public int getDishPrice(string dishName)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var price = from c in connect.Dishes where c.M_Name == dishName && c.CHEF_Id == getID(getEmail()) select c.M_Points;

            return price.First<Int32>();

        }


        public string getDishDescription(string dishName)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var desc = from c in connect.Dishes where c.M_Name == dishName && c.CHEF_Id == getID(getEmail()) select c.M_Description;

            return desc.First<String>();
        }



        public void updateMeal(string oldName, string mealName, string Price, string Ingredients, string Description, string Image)
        {
           
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var mealId = from c in connect.Dishes where c.M_Name == oldName && c.CHEF_Id == getID(getEmail()) select c.M_Id;

            var meal = connect.Dishes.FirstOrDefault(m => m.M_Id == mealId.First<Int32>());
            int intPrice = Int32.Parse(Price);
            if (meal != null)
            {
                meal.M_Name = mealName;
                meal.M_Points = intPrice;
                meal.M_Ingridients = Ingredients;
                meal.M_Description = Description;
                meal.M_MealImage = Image;
            }

            connect.SubmitChanges();
        }

        /**
         *A method for deleting a meal from the database 
         */
        public bool DeleteMeal(string mealName)
        {
            bool isDeleted = false;

            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var mealId = from c in connect.Dishes where c.M_Name == mealName && c.CHEF_Id == getID(getEmail()) select c.M_Id;
            var cat = from c in connect.DishCatBridges where c.M_ID == mealId.First() select c;

            var meal = from t in connect.Dishes
                       where t.M_Id == mealId.First()
                       select t;

            if (meal.Any())
            {
                connect.DishCatBridges.DeleteOnSubmit(cat.First());
                connect.Dishes.DeleteOnSubmit(meal.First());
                connect.SubmitChanges();

                isDeleted = true;
            }

            return isDeleted;
        }

        public void upload(string name, string ingredients, string description, string mealImage, int price,string category)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();
           // var chef = from i in connect.Accounts where i.U_Email == getEmail() select i.U_ID;
            var cat = from i in connect.MealCategories where i.Cat_Name == category select i.Cat_ID;
            var dish = from i in connect.Dishes where i.M_Name == name && i.CHEF_Id == getID(getEmail()) select i.M_Id;
            Dish newMeal = new Dish()
            {
                M_Name = name,
                M_Ingridients = ingredients,
                M_Description = description,
                M_Points = price,
                M_MealImage = mealImage,
                CAT_Id = cat.First(),
                CHEF_Id = getID(getEmail())
            };

            connect.Dishes.InsertOnSubmit(newMeal);
            connect.SubmitChanges();
            
            DishCatBridge mealCategory = new DishCatBridge()
            {
                M_ID = dish.First(),
                Cat_ID = cat.First()
            };
            connect.DishCatBridges.InsertOnSubmit(mealCategory);
            connect.SubmitChanges();
            
        }



        /**
     * Accessor and mutator methods
     */



        public int getMealId(string email,string dishName)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var id = from i in connect.Dishes
                     where i.CHEF_Id == getID(email) && i.M_Name == dishName
                     select i.M_Id;

            return id.First();
        }

        public void setType(string type)
        {
            this.type = type;
        }

        public string getType()
        {
            return type;
        }

        public void setBio(string bio)
        {
            this.bio = bio;
        }

        public string getBio()
        {
            return bio;
        }

        public void setRating(int rating)
        {
            this.rating = rating;
        }

        public int getRating()
        {
            return rating;
        }

        public void setID(int id)
        {
            this.id = id;
        }

        public int getID()
        {
            return id;
        }

        public int getID(string email)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();
            var id = from i in connect.Accounts where i.U_Email == email select i.U_ID;
            return id.First();
        }





    }




}