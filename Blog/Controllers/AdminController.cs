using System;
using System.Linq;
using System.Web.Mvc;
using Blog.Models;
using System.Text;
using System.IO;
using System.Security.Cryptography;
namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private static readonly string _passPhrase = "helloworld";
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private  const string InitVector = "jerryyu1xyelul88";
        // This constant is used to determine the keysize of the encryption algorithm
        private   const int Keysize = 256;
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Member member)
        {
            if (ModelState.IsValid)
            {
                using (BlogContext db = new BlogContext())
                {
                    string hashPass=EncryptString(member.Password,_passPhrase);
                    var v = db.Members.Where(a => a.UserName.Equals(member.UserName) && a.Password.Equals(hashPass)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LoggedUserID"] = v.UserId.ToString();
                        Session["LoggedUserName"] = String.IsNullOrEmpty(v.NickName) ? v.UserName : v.NickName;
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            return View(member);
        }
        [HttpGet]

        public ActionResult Signup()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup( Member member)
        {
            if (ModelState.IsValid)
            {
                using (BlogContext db = new BlogContext())
                {
                    member.UserId = null;
                    member.IsActive = 1;
                    member.Password=EncryptString(member.Password, _passPhrase);
                    db.Members.Add(member);
                    db.SaveChanges();
                }
                return Content("Congratulation ! New Member "+(String.IsNullOrEmpty(member.NickName) ? member.UserName : member.NickName)+". Please <b><a href='/Admin/Login'>Click Here to Login</a></b>");
            }
            else
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //Encrypt
        public  string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public  string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(Keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
