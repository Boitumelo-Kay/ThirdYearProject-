using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dial_A_Chef_WebApi;
using Dial_A_Chef_WebApi.Models;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class MealsController : ApiController
    {
        // GET api/Meals
        public List<string> GetMeals()
        {
            Meal meal = new Meal();

            return meal.getMeals();
        }

        public string GetMeals(int id)
        {
            Meal meal = new Meal();

            return meal.getMeals(id);
        }

    }
}
