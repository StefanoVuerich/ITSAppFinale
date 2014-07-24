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
            string imagePath = "";

            if (file != null)
            {
                imagePath = "http://192.168.102.2/images/article/";
                string pic = titolo + "_img.jpeg";
                imagePath += pic;
                string location = (Server.MapPath("~/images/article/")) + pic;
                bool isImageInserted = ImageController.InsertImage(file, location);
            }
            if (type == "evento")
            {
                SQLEventsRepository _rep = new SQLEventsRepository();
                int eventId = _rep.Post(new Event()
                {
                    DataEvento = form["dataEvento"].ToString(),
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
        [HttpPut]
        [Route("api/editentity/{article}")]
        public ActionResult EditEntity()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                //Use the following properties to get file's name, size and MIMEType
                int fileSize = file.ContentLength;
                string immagineVecchiaENuova = Request.Files.AllKeys[i];
                string immagineVecchia = immagineVecchiaENuova.Substring(immagineVecchiaENuova.IndexOf("$") + 1, immagineVecchiaENuova.LastIndexOf("$") - 1);
                string immagineNuova = immagineVecchiaENuova.Substring(immagineVecchiaENuova.LastIndexOf("$") + 1);
                ImageController.DeleteImage(immagineVecchia);
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Server.MapPath("~/images/article/") + immagineNuova); //File will be saved in application root
            }
            return RedirectToAction("EventManager", "Manager");
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