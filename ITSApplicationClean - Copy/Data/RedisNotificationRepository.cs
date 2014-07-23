using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectModel;
using ServiceStack.Redis;
using System.Diagnostics;

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
                Debug.Assert(allNotification.Count >= 5, "Inserire almeno cinque news.");
                List<string> unreceivedNotification;

                if (lastReceivedNotification == "" || allNotification.IndexOf(lastReceivedNotification) == allNotification.Count - 1)
                {
                    unreceivedNotification = new List<string>();
                    for (int x = allNotification.Count - 5; x < allNotification.Count ; x++)
                    {
                        unreceivedNotification.Add(allNotification[x]);
                    }
                    return unreceivedNotification;
                }
                else if ((allNotification.Count -1 )  - allNotification.IndexOf(lastReceivedNotification) < 5)
                {
                    unreceivedNotification = new List<string>();
                    for (int x = allNotification.Count - 5; x < allNotification.Count; x++)
                    {
                        unreceivedNotification.Add(allNotification[x]);
                    }
                    return unreceivedNotification;
                }
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
        private static RedisClient GetRedisClient ()
        {
            return new RedisClient("192.168.102.2", 6379);
        }
    }
}
