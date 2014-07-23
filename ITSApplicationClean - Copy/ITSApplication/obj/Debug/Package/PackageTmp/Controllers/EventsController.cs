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
        [Route("api/events/{lastFiveEvents}")]
        public IEnumerable<Event> GetAll(string lastFiveEvents)
        {
            return sqlEventsRepository.GetLastFive();
        }
        [AllowAnonymous]
        [Route("api/event/{id:int}")]
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
        public void Put(Event event_obj)
        {
            //event_obj.Id = id;
            if (!sqlEventsRepository.Put(event_obj))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        [Route("api/events/del/{id:int}")]
        public void Delete(int id)
        {
            Event event_obj = sqlEventsRepository.Get(id);
            string image = event_obj.Titolo + "_img.jpeg";
            if(event_obj == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            sqlEventsRepository.Delete(id);
            ImageController.DeleteImage(image);
        }
    }
}
