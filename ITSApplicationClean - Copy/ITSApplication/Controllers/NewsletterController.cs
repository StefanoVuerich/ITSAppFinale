using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ITSApplication.Controllers
{
    [AllowAnonymous]
    public class NewsletterController : ApiController
    {
        [Route("newletter")]
        public bool Post(string email)
        {
            return true;
        }
    }
}
