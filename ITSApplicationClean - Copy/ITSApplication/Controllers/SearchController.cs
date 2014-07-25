using Data;
using ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ITSApplication.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SearchController : ApiController
    {
        private SQLEventsRepository eventRepository = new SQLEventsRepository();
        private SQLNewsRepository newsRepository = new SQLNewsRepository();

        // GET: api/Search
        [Route("search/{keyWord}")]
        public IEnumerable<Article> Get(string keyWord)
        {
            List<Article> articoliRicercati = new List<Article>();

            foreach (Event event_obj in eventRepository.Search(keyWord))
            {
                articoliRicercati.Add(event_obj);
            }

            foreach (News news in newsRepository.Search(keyWord))
            {
                articoliRicercati.Add(news);
            }

            return articoliRicercati.OrderByDescending(e => e.DataPubblicazione);
        }
    }
}