using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data;
using ObjectModel;
using System.Web.Http.Cors;

namespace ITSApplication.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        SQLEventsRepository sqlEventsRepository = new SQLEventsRepository();
        [AllowAnonymous]
        public IEnumerable<Event> GetAll()
        {
            return sqlEventsRepository.GetAll();
        }
        [AllowAnonymous]
        [Route("api/event/{id}")]
        public Event Get(int id)
        {
            Event event_obj = sqlEventsRepository.Get(id);
            if (event_obj == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return event_obj;
        }
        public HttpResponseMessage Post(Event eventObj)
        {
            sqlEventsRepository.Post(eventObj);
            var response = Request.CreateResponse<Event>(HttpStatusCode.Created, eventObj);
            return response;
        }
        public void Put(int id, Event event_obj)
        {
            event_obj.Id = id;
            if (!sqlEventsRepository.Put(event_obj))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        public void Delete(int id)
        {
            Event event_obj = sqlEventsRepository.Get(id);
            if(event_obj == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            sqlEventsRepository.Delete(id);
        }
    }
}
