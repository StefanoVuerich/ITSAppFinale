using Data;
using ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ITSApplication.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewsController : ApiController
    {
        private SQLNewsRepository sqlNewsRepository = new SQLNewsRepository();

        [AllowAnonymous]
        [Queryable]
        public IEnumerable<News> GetAll()
        {
            return sqlNewsRepository.GetAll();
        }

        [AllowAnonymous]
        [Route("api/news/{lastFiveNews}")]
        public IEnumerable<News> GetAll(string lastFiveNews)
        {
            return sqlNewsRepository.GetLastFive();
        }

        [AllowAnonymous]
        [Route("api/news/{id:int}")]
        public News Get(int id)
        {
            News news = sqlNewsRepository.Get(id);
            if (news == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return news;
        }

        public HttpResponseMessage Post(News news)
        {
            sqlNewsRepository.Post(news);
            var response = Request.CreateResponse<News>(HttpStatusCode.Created, news);
            return response;
        }

        public void Put(News news)
        {
            //news.Id = id;
            if (!sqlNewsRepository.Put(news))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("api/news/del/{id:int}")]
        public void Delete(int id)
        {
            News news = sqlNewsRepository.Get(id);
            string image = news.Titolo + "_img.jpeg";
            if (news == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            sqlNewsRepository.Delete(id);
            ImageController.DeleteImage(image);
        }
    }
}