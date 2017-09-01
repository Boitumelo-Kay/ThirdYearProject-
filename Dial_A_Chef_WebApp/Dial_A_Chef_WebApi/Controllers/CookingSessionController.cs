using Dial_A_Chef_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dial_A_Chef_WebApi.Controllers
{
    public class CookingSessionController : ApiController
    {
        CookingSessionModel Session { get; set; }
        public string StartSession()
        {
            DateTime date = Session.StartSession();

            return date.ToString();
        }



    }
}
