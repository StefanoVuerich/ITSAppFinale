using ServiceStack.Redis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Data
{
    public class RedisNotificationRepository
    {
        public IEnumerable<Dictionary<string, string>> GetAll()
        {
            List<Dictionary<string, string>> tutteNotifiche2 = new List<Dictionary<string, string>>();
            using (var redis = GetRedisClient())
            {
                List<string> notifications = redis.GetAllKeys();

                var client = redis.As<string>();

                Dictionary<string, string> obj;

                foreach (string s in notifications)
                {
                    var hash = client.GetHash<string>(s);

                    obj = hash.GetAll();

                    tutteNotifiche2.Add(obj);
                }
            }
            return tutteNotifiche2;
        }

        public static void Insert(string key)
        {
            using (var redis = GetRedisClient())
            {
                var client = redis.As<string>();
                var list = client.Lists["NotificationList"];
                list.Add(key);
            }
        }

        public static void Delete(string key)
        {
            using (var redis = GetRedisClient())
            {
                var client = redis.As<string>();
                client.Lists["NotificationList"].Remove(key);
            }
        }

        public static IEnumerable<string> GetLastNotifications(string lastReceivedNotification)
        {
            using (var redis = GetRedisClient())
            {
                var client = redis.As<string>();
                List<string> allNotification = client.Lists["NotificationList"].ToList();
                int notificationCount = allNotification.Count;
                int notificheDaInviare = notificationCount;
                if (notificationCount >= 5)
                {
                    notificheDaInviare = 5;
                }
                Debug.Assert(notificationCount >= notificheDaInviare, "Le notifiche da iviare superano le notifiche totali");
                List<string> unreceivedNotification;
                // caso in cui la stringa di notifiche ricevute è vuota oppure le notifiche ricevute sono pari alle notifiche totali
                if (lastReceivedNotification == "" || allNotification.IndexOf(lastReceivedNotification) == allNotification.Count - 1)
                {
                    unreceivedNotification = new List<string>();
                    for (int x = notificationCount - notificheDaInviare; x < notificationCount; x++)
                    {
                        unreceivedNotification.Add(allNotification[x]);
                    }
                    return unreceivedNotification;
                }
                // caso in cui sono state immesse meno di x(5) notifiche dall'ultimo accesso
                else if ((allNotification.Count - 1) - allNotification.IndexOf(lastReceivedNotification) < notificheDaInviare)
                {
                    unreceivedNotification = new List<string>();
                    for (int x = notificationCount - notificheDaInviare; x < notificationCount; x++)
                    {
                        unreceivedNotification.Add(allNotification[x]);
                    }
                    return unreceivedNotification;
                }
                // caso in cui vengono inviate tutte le notifiche, dall'ultima ricevuta fino al count
                else
                {
                    unreceivedNotification = new List<string>();
                    int index = allNotification.IndexOf(lastReceivedNotification);
                    int lunghezzaLista = allNotification.Count;

                    for (int x = index + 1; x < lunghezzaLista; x++)
                    {
                        unreceivedNotification.Add(allNotification[x]);
                    }
                    return unreceivedNotification;
                }
            }
        }

        private static RedisClient GetRedisClient()
        {
            return new RedisClient("192.168.102.2", 6379);
        }
    }
}