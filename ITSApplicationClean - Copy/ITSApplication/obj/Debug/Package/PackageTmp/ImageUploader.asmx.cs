using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ITSApplication
{
    /// <summary>
    /// Summary description for ImageUploader1
    /// </summary>
    //[ (Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ImageUploader1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string UploadImage(string s)
        {
            return "ok";
        }
    }
}
