using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data;
using ObjectModel;

namespace ITSApplication.Controllers
{
    public class RedisController : ApiController
    {
        RedisNotificationRepository redisNewsRepository = new RedisNotificationRepository();
        [Route("redis/{lastReadedNews}")]
        public IEnumerable<string> Get(string lastReadedNews)
        {
            return RedisNotificationRepository.GetLastNotifications(lastReadedNews);
        }
    }
}
