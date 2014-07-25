using Data;
using System.Collections.Generic;
using System.Web.Http;

namespace ITSApplication.Controllers
{
    public class RedisController : ApiController
    {
        private RedisNotificationRepository redisNewsRepository = new RedisNotificationRepository();

        [Route("redis/{lastReadedNews}")]
        public IEnumerable<string> Get(string lastReadedNews)
        {
            return RedisNotificationRepository.GetLastNotifications(lastReadedNews);
        }
    }
}