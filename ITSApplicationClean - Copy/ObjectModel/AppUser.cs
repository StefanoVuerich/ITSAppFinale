using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ObjectModel
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Salt { get; set; }
        public int MyProperty { get; set; }
        public AppUser(string username, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            } else  {
                var encryptedPass = Encoding.Unicode.GetBytes(password);
                string hash = MD5.Create().ToString();
                }
            

            this.Username = username;
            this.Password = password;
        }
        public static string getToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }

    
}
