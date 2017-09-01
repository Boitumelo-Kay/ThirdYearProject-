
using Dial_A_Chef_WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial_A_Chef_WebApi.Models
{
    public class Meal
    {
        public string M_Name { get; set; }
        public List<string> M_Ingredients { get; set; }
        public string M_Description { get; set; }
        public string M_Image { get; set; }
        public int M_Points { get; set; }
        public int ChefId { get; set; }

        public int CatId { get; set; }

       

        public Meal()
        {
            M_Name = null;
            M_Ingredients = null;
            M_Description = null;
            M_Image = null;
            M_Points = 0;
        }

        public Meal(string name, List<string> mIngredients,string mDescription,string mImage,int mPoints,int id, string catName)
        {
            this.M_Name = name;
            this.M_Ingredients = mIngredients;
            this.M_Description = mDescription;
            this.M_Points = mPoints;
            this.M_Image = mImage;
            this.ChefId = id;
            CatId = this.GetCatId(catName);

        }

        public bool UploadMeal()
        {
            bool isUpload = false;
            Dish newDish = new Dish()
            {
                M_Name = getName(),
                M_Points = getPoints(),
                M_Description = getDescription(),
                M_Ingridients = this.M_Ingredients.ToString(), 
                M_MealImage = getImage(),
                CHEF_Id = getChefId(),
                CAT_Id = CatId
            };

            ubDatabaseDataContext data = new ubDatabaseDataContext();

            data.Dishes.InsertOnSubmit(newDish);
            data.SubmitChanges();

            DishCatBridge dishCat = new DishCatBridge()
            {
                M_ID = newDish.M_Id,
                Cat_ID = CatId
            };

            data.DishCatBridges.InsertOnSubmit(dishCat);
            data.SubmitChanges();

            var dishExists = from d in data.Dishes
                             where d.M_Id == newDish.M_Id
                             select d;

            if (dishExists.FirstOrDefault() != null)
            {
                isUpload = true;
            }
            return isUpload;
        }

        public void setChefId(int id)
        {
            this.ChefId = id;
        }

        public int getChefId()
        {
            return this.ChefId;
        }

        public void setName(string name)
        {
            M_Name = name;
        }

        public string getName()
        {
            return M_Name;
        }

        public void setDescription(string description)
        {
            this.M_Description = description;
        }

        public string getDescription()
        {
            return M_Description;
        }

        public void setImage(string image)
        {
            this.M_Image = image;
        }

        public string getImage()
        {
            return this.M_Image;
        }

        public void setPoints(int points)
        {
            this.M_Points = points;
        }

        public int getPoints()
        {
            return M_Points;
        }


        /*
            Helper function that finds the dish's category id
            Queries the category name
        */
        public int GetCatId(String catName)
        {
            int id = -1;
            ubDatabaseDataContext data = new ubDatabaseDataContext();
             
            var cat = from c in data.MealCategories
                      where c.Cat_Name == catName
                      select c;

            try
            {
                if (cat.FirstOrDefault() != null)
                {
                    id = cat.FirstOrDefault().Cat_ID;
                }
            }
            catch (NullReferenceException e)
            {
                throw new NullReferenceException();
            }
            return id;
        }


        public List<String> getMeals()
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var meals = from i in connect.Dishes select i;

            List<String> mealList = new List<String>();

            foreach (var i in meals)
            {
                int mealId = i.M_Id;
                string mealName = i.M_Name;
                int mealPrice = i.M_Points;
                string mealIngredients = i.M_Ingridients;
                string mealDescription = i.M_Description;
                string mealImage = i.M_MealImage;

                string meal = mealId + "-" + mealName + "-" + mealIngredients + "-" + mealDescription + "-" + mealPrice + "-" + mealImage;

                mealList.Add(meal);

            }

            return mealList;
        }

        public String getMeals(int mealId)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var meal = from i in connect.Dishes where i.M_Id == mealId select i;


            return meal.First().M_Id + "-" + meal.First().M_Name + "-" + meal.First().M_Ingridients + "-" + meal.First().M_Description + "-" + meal.First().M_Points + "-" + meal.First().M_MealImage;

        }


        public void placeOrder(int dishId, string cusEmail, string prepMethod)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();


            var chefId = from i in connect.Dishes where i.M_Id == dishId select i.CHEF_Id;


            Order order = new Order()
            {
                O_Date = DateTime.Now,
                Dish_ID = dishId,
                Cust_ID = getCusId(cusEmail),
                Prep_Method = prepMethod,
                Chef_ID = chefId.First(),
                Delivery_Method = "Chef",
                IsAccepted = false,
                SeenByChef = false

            };

            connect.Orders.InsertOnSubmit(order);
            connect.SubmitChanges();

        }

        private int getCusId(string email)
        {
            ubDatabaseDataContext connect = new ubDatabaseDataContext();

            var id = from i in connect.Accounts where i.U_Email == email select i.U_ID;

            return id.First();
        }



    }
}