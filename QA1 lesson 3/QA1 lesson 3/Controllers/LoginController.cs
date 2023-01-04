using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using QA1_lesson_3.Models;
using System.Xml.Linq;
using System;
using System.Runtime.CompilerServices;

namespace QA1_lesson_3.Controllers
{
    public class LoginController : Controller
    {
        private string _key;
        public LoginController()
        {
            _key = "E546C8DF278CD5931069B522E695D4F2";
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                var model = GetProvinces();
                var p = new Province();
               
                return View(model);
            }
            else
                return Redirect("home");
        }
        [HttpPost]
        public IActionResult doLogin(Logindata login)
        {
            LoginResponse res = new LoginResponse();
            if (login != null)
            {
                User? usr = checkLogin(login);
                if (usr != null)
                {
                    var passHash = EncryptString(login.Password, _key);
                    var decrytePass = DecryptString(usr.Password, _key);
                    if (decrytePass == login.Password)
                    {
                        res.message = "Complete";
                        res.Success = true;
                        res.Users = usr;
                        HttpContext.Session.SetString("Username", usr.Username);
                        HttpContext.Session.SetString("Fullname", usr.Fullname);

                    }
                    else
                    {
                        res.message = "Fail";
                        res.Success = false;
                        res.Users = null;
                    }

                }
                else
                {
                    res.message = "Fail";
                    res.Success = false;
                }
            }
            else
            {
                res.message = "None data";
                res.Success = false;
            }
            return Json(res);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Fullname");
            LoginResponse res = new LoginResponse();
            res.message = "";
            res.Success = true;
            return Json(res);
        }
        public IActionResult Register(IFormCollection f)
        {
           
            User u = new User();
            u.Fullname = f["txtFullname"].ToString();
            u.Username = f["txtUsername1"].ToString();
            u.Password = f["txtPassword1"].ToString();
            u.City = f["txtCity"].ToString();
            u.State = f["selState"].ToString();
            u.Zip = f["txtZip"].ToString();

            var obj = CreateUser(u);
            return View(obj);

        }
        public User? checkLogin(Logindata login)
        {
            User? usr = new User();
            if (login != null)
            {
                string cnStr = "Server =LAPTOP-SMGF68QR;Database = LTWeb;User id=mhuy;password =123; ";
                SqlConnection cnn = new SqlConnection(cnStr);
                try
                {
                    cnn.Open();
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.Connection = cnn;
                    string sql = "select * from Users";
                    sql += " where Username ='" + login.Username + "'";
                    //sql += " and Password ='" + login.Password + "'";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        usr.Id= int.Parse(reader["ID"].ToString());
                        usr.Username = reader["Username"].ToString();
                        usr.Password = reader["Password"].ToString();
                        if (reader["LastLogin"] != null && reader["LastLogin"].ToString() != "")
                        {
                            usr.Lastlogin = DateTime.Parse(reader["Lastlogin"].ToString());
                        }
                        usr.Fullname = reader["Fullname"].ToString();
                    }
                    reader.Close();
                    if (usr != null && !(usr.Id > 0))
                    {
                        usr = null;
                    }
                }
                catch (Exception e)
                {
                    usr = null;
                }
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return usr;
        }
        public IActionResult change_pass(string username, string oldPass, string newPass)
        {
            var obj = changePass(username, oldPass, newPass);
            return Json(obj);
        }
       
        public static string GiaiMa(string cipherText)
        {
            var keyString = "E546C8DF278CD5931069B522E695D4F2";
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        private object? changePass(string uid, string oPass, string nPass)
        {
            try
            {
                var db = new LtwebContext();
                var usr = db.Users.Where(x => x.Username == uid).FirstOrDefault();
                if (usr == null)
                {
                    return new
                    {
                        success = false,
                        message = "Username has not exited!!"
                    };
                }
                else
                {
                    var cPass = DecryptString(usr.Password, _key);
                    if(cPass != oPass)
                    {
                        return new
                        {
                            success = false,
                            message = "Wrong Pass!!"
                        };
                    }
                    else
                    {
                        var hashPass = EncryptString(nPass, _key);
                        usr.Password = hashPass;
                        db.Users.Update(usr);
                        db.SaveChanges();
                        HttpContext.Session.Remove("Username");
                        HttpContext.Session.Remove("Fullname");
                        return new
                        {
                            success = true,
                            message = "Success change pass"
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }
        private string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        private string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        private object? GetProvinces()
        {
            var db = new LtwebContext();
            var res = db.Provinces.ToList();
            return res;
        }
        private object? CreateUser(User u)
        {
            try
            {
                var db = new LtwebContext();
                var usr = db.Users.Where(x => x.Username == u.Username).FirstOrDefault();
                if (usr != null)
                {
                    return new
                    {
                        success = false,
                        message = "Username has not exited!!",
                        data = ""
                    };
                }
                else
                {
                
                    var hashpass = EncryptString(u.Password, _key);
                    u.Password = hashpass;
                    db.Users.Add(u);
                    db.SaveChanges();
                    var t = GetProvinceById(u.State);
                    return new
                    {
                        success = true,
                        message = "Tạo User thành công",
                        data = u,
                        province= t
                    };
                       
                }
        }          
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        private Province? GetProvinceById(string id)
        {
            int iid = int.Parse(id);
            var db = new LtwebContext();
            var res = db.Provinces.Where(x => x.Id == iid).FirstOrDefault();
            return res;
        }
    }

    
    public class Logindata
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {     
        public bool Success { get; set; }
        public string message { get; set; }
        public User? Users { get; set; }
    }
    public class Users
    {
        public int? ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }

        public DateTime? LastLogin { get; set; }
    }
   
}
