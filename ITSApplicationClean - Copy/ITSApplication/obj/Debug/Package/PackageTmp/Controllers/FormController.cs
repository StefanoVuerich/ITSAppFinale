using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using ObjectModel;
using NotificationSystem;

namespace ITSApplication.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        [HttpPost]
        public ActionResult FormHandler(HttpPostedFileBase file, FormCollection form)
        {
            string type = form["type"].ToString().Substring(9).ToLower();
            string titolo = form["titolo"].ToString();
            string testo = form["testo"].ToString();
            string imagePath = "http://192.168.102.2/images/article/";

            if (file != null)
            {
                string fileName = file.FileName;
                string extension = fileName.Substring(fileName.Length - 3);
                string pic = titolo + "_img." + extension;
                imagePath += pic;

                //string path = System.IO.Path.Combine(Server.MapPath("~/images/profile"), pic);
                file.SaveAs(Server.MapPath("~/images/article/") + pic);
            }

            if (type == "evento")
            {
                SQLEventsRepository _rep = new SQLEventsRepository();
                int eventId = _rep.Post(new Event()
                {
                    Titolo = titolo,
                    Testo = testo,
                    UrlFoto = imagePath
                });

                Notificate("Event_id_" + eventId);
                return RedirectToAction("EventManager", "Manager");
            }
            else if (type == "news")
            {
                SQLNewsRepository _rep = new SQLNewsRepository();
                int newsId = _rep.Post(new News()
                {
                    Titolo = titolo,
                    Testo = testo,
                    UrlFoto = imagePath
                });
                Notificate("News_id_" + newsId);
                return RedirectToAction("NewsManager", "Manager");
            }
            return null;
        }

        public bool Notificate(string notification)
        {
            try
            {
                NotificationHub.Notification(notification);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception throw {0}", e.Message);
                return false;
            }
        }
    }
}