using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ObjectModel;

namespace ITSApplication.Controllers
{
    public class UserController : ApiController
    {
        [Route("user/{key}")]
        public string Post(string key)
        {
            string username = key.Substring(key.LastIndexOf('=') + 1);

            if (key == "stefano")
            {
                return AppUser.getToken();
            }
            else
            {
                return "denied";
            }
        }
    }
}
