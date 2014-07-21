using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data;
using ObjectModel;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace ITSApplication.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewsController : ApiController
    {
        SQLNewsRepository sqlNewsRepository = new SQLNewsRepository();
        [AllowAnonymous]
        [Queryable]
        public IQueryable<News> GetAll()
        {
            return sqlNewsRepository.GetAll().AsQueryable();
        }
        [AllowAnonymous]
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
        public void Delete(int id)
        {
            News news = sqlNewsRepository.Get(id);
            if(news == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            sqlNewsRepository.Delete(id);
        }
    }
}
