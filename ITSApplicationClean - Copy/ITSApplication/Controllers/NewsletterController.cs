using Data;
using ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ITSApplication.Controllers
{
    [AllowAnonymous]
    public class NewsletterController : ApiController
    {
        private NewsletterRepository _rep = new NewsletterRepository();

        [HttpPost]
        [Route("newsletter")]
        public HttpResponseMessage Post(NewsletterEmail emailAdress)
        {
            bool isEmail = Regex.IsMatch(emailAdress.EmailAdress.Trim(), @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z");

            if (isEmail)
            {
                bool isInserted = _rep.Post(emailAdress.EmailAdress);

                if (isInserted)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}