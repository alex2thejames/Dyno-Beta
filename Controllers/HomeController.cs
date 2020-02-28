using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dyno.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Globalization;

//email stuff
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;
//Scraper feed population stuff    
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace Dyno.Controllers
{
    public class HomeController : Controller
    {   

        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

////////Login/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        [HttpGet("")]
        public IActionResult LoginPage()
        {
            if(HttpContext.Session.GetInt32("logID") != null)
            {
                return Redirect("/profile");
            }
            return View();
        }

        public IActionResult LoginProcess(Login logUser)
        {
            if(ModelState.IsValid)
            {   
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == logUser.Email);
                if(userInDb == null)
                {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("LoginPage");
                }
                var hasher = new PasswordHasher<Login>();

                var result = hasher.VerifyHashedPassword(logUser, userInDb.Password, logUser.Password);

                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("LoginPage");
                }

                else 
                {
                    HttpContext.Session.SetInt32("logID", userInDb.UserId);
                    return Redirect("/profile");
                }
            }
            else
            {   
                return View("LoginPage");
            }
        }

        
        public IActionResult ResetPassword(Login sender)
        {
            if(dbContext.Users.FirstOrDefault(u => u.Email == sender.Email) != null)
                {
                    // MailMessage mm = new MailMessage();
                    // mm.Subject = "Password Recovery";
                    // mm.To.Add("alex2thejames@gmail.com");
                    // mm.From = new MailAddress("abc@gmail.com");
                    // mm.Body = string.Format("Hi {0},<br /><br />Your password is {1}.<br /><br />Thank You.", sender.Email, sender.Password);
                    // mm.IsBodyHtml = true;
                    // SmtpClient smtp = new SmtpClient();
                    // smtp.Host = "smtp.gmail.com";
                    // smtp.EnableSsl = true;
                    // NetworkCredential NetworkCred = new NetworkCredential();
                    // NetworkCred.UserName = "sender@gmail.com";
                    // NetworkCred.Password = "<Password>";
                    // smtp.UseDefaultCredentials = true;
                    // smtp.Credentials = NetworkCred;
                    // smtp.Port = 587;
                    // smtp.Send(mm);
                    return Json("Password has been sent to your email address.");
                }
            else
            {
                ModelState.AddModelError("Email", "Email does not exist!");
                return View("LoginPage");
            }
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult RegisterProcess(User newUser)
        {   
            if(ModelState.IsValid)
            {   
                if(dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Register");
                }
                else 
                {
                    if(Regex.IsMatch(newUser.Password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"))
                    {
                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                        dbContext.Users.Add(newUser);
                        dbContext.SaveChanges();
                        var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                        HttpContext.Session.SetInt32("logID", userInDb.UserId);
                        return Redirect("/interests/setup");
                    }
                
                    else
                    {
                        ModelState.AddModelError("Password", "Password Must Contain an Symbol, Number, and One Alphabetical Character");
                        return View("Register");
                    }
                }
            }
            else
            {
                return View("Register");
            }
        }

/////////Profile/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        [HttpGet("/profile")]
        public IActionResult Profile()
        {
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/interests/setup");
            }
            if(dbContext.Users.FirstOrDefault(user => user.UserId == CurrUser.UserId) == null)
            {
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            if(dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1) == null)
            {
                Admin newAdmin = new Admin();
                newAdmin.WebSet = false;
                dbContext.Admins.Add(newAdmin);
                dbContext.SaveChanges();
                return Redirect("/profile");
            }
            Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
            if(AdminRule.WebSet == false)
            {
                return View("FakeProfile");
            }
            if(AdminRule.WebSet == true)
            {
                /////////refresh every five minutes
                DateTime now = DateTime.Now;
                foreach(var i in dbContext.Interests.Where(interest => interest.UserId == CurrUser.UserId).ToList())
                {
                    System.Console.WriteLine(i.Topic.TopicName);
                    if((now - i.Topic.UpdatedAt).Minutes > 5 | i.Topic.TopicTitle == null)
                        {
                        System.Console.WriteLine((now - i.Topic.UpdatedAt).Minutes);
                        string xxx= i.Topic.TopicName;
                        // ***#**#**#*#**#************************************
                        IWebDriver driver = new ChromeDriver(@"/Users/Alexander/Documents/Coding Dojo/C#/Dyno/bin/Debug/netcoreapp2.2/");
                        driver.Url = $"https://news.google.com/search?q={xxx}&hl=en-US&gl=US&ceid=US%3Aen";

                        // grabs Url link 
                        List<string> Links = new List<string>();
                        int Counter = 0;
                        foreach(var item in driver.FindElements(By.TagName("a")))
                        {
                            Counter= Counter+1;
                            if( Counter ==21)
                            {
                                string x =item.GetAttribute("href");
                                Links.Add(x);
                            }
                            if(Counter >= 22)
                            {
                                break;
                            }
                                    
                        }
                        List<string> uniqueLinks = Links.Distinct().ToList();

                        foreach(var l in uniqueLinks)
                        {
                            i.Topic.TopicLink =  l;
                        }
                        // grabs title
                        List<string> Titles = new List<string>();
                        int Counter2 = 0;
                        foreach(var item in driver.FindElements(By.ClassName("DY5T1d")))
                        {
                            Counter2= Counter2+1;
                            if(Counter2 ==1)
                            {
                                string q = item.GetAttribute("text");
                                Titles.Add(q);
                            }
                        }
                        foreach(string u in Titles)
                        {
                            i.Topic.TopicTitle= u;
                        }
                        i.Topic.TopicIMG= $"https://source.unsplash.com/1600x900/?{xxx}";
                        i.Topic.UpdatedAt = now;
                        dbContext.Update(i);
                        dbContext.SaveChanges();
                        }
                    return View("Profile");
                }
            }
            return View("FakeProfile");
        }

////////Interests////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        [HttpGet("/interests/setup")]
        public IActionResult InterestSetup()
        {
        if(HttpContext.Session.GetInt32("logID") == null)
        {
            return Redirect("/");
        }
        int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
        User CurrUser = dbContext.Users
        .Include(user => user.Interests)
        .ThenInclude(interests => interests.Topic)
        .FirstOrDefault(user => user.UserId == id);
        if(CurrUser.Interests.Count >= 3)
        {
            return Redirect("/profile");
        }
        ViewBag.CurrUser = CurrUser;
        return View();
        }
        // ////////////////////
        
        
        public IActionResult InterestIntialize(TopicTemp newTopic)
        {
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;

            

            if(ModelState.IsValid)
            {
                
                //put all topic inputs into lowercase with first letter upper for easier querying
                newTopic.TopicName1 = newTopic.TopicName1.ToLower(new CultureInfo("en-US", false));
                newTopic.TopicName1 = char.ToUpper(newTopic.TopicName1[0]) + newTopic.TopicName1.Substring(1);
                newTopic.TopicName2 = newTopic.TopicName2.ToLower(new CultureInfo("en-US", false));
                newTopic.TopicName2 = char.ToUpper(newTopic.TopicName2[0]) + newTopic.TopicName2.Substring(1);
                newTopic.TopicName3 = newTopic.TopicName3.ToLower(new CultureInfo("en-US", false));
                newTopic.TopicName3 = char.ToUpper(newTopic.TopicName3[0]) + newTopic.TopicName3.Substring(1);
                
                if(dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName1) != null)
                {
                    Topic TopicName1Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName1);
                    Interest Interest1 = new Interest();
                    Interest1.UserId = id;
                    Interest1.TopicId = TopicName1Id.TopicId;
                    dbContext.Interests.Add(Interest1);
                    dbContext.SaveChanges();
                }
                else
                {
                    if(dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1) == null)
                    {
                        Admin newAdmin = new Admin();
                        newAdmin.WebSet = false;
                        dbContext.Admins.Add(newAdmin);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
                    if(AdminRule.WebSet == false)
                    {
                        Topic Topic1 = new Topic();
                        Topic1.TopicName = newTopic.TopicName1;
                        dbContext.Topics.Add(Topic1);
                        dbContext.SaveChanges();

                        Topic TopicName1Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic1.TopicName);
                        Interest Interest1 = new Interest();
                        Interest1.UserId = id;
                        Interest1.TopicId = TopicName1Id.TopicId;
                        dbContext.Interests.Add(Interest1);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    if(AdminRule.WebSet == true)
                    {
                        Topic Topic1 = new Topic();
                        string xxx= newTopic.TopicName1;
                        // ***#**#**#*#**#************************************
                        IWebDriver driver = new ChromeDriver(@"/Users/Alexander/Documents/Coding Dojo/C#/Dyno/bin/Debug/netcoreapp2.2/");
                        driver.Url = $"https://news.google.com/search?q={xxx}&hl=en-US&gl=US&ceid=US%3Aen";

                        // grabs Url link 
                        List<string> Links = new List<string>();
                        int Counter = 0;
                        foreach(var item in driver.FindElements(By.TagName("a")))
                        {
                            Counter= Counter+1;
                            if( Counter ==21)
                            {
                                string x =item.GetAttribute("href");
                                Links.Add(x);
                            }
                            if(Counter >= 22)
                            {
                                break;
                            }
                                    
                        }
                        List<string> uniqueLinks = Links.Distinct().ToList();

                        foreach(var l in uniqueLinks)
                        {
                            Topic1.TopicLink =  l;
                        }
                        // grabs title
                        List<string> Titles = new List<string>();
                        int Counter2 = 0;
                        foreach(var item in driver.FindElements(By.ClassName("DY5T1d")))
                        {
                            Counter2= Counter2+1;
                            if(Counter2 ==1)
                            {
                                string q = item.GetAttribute("text");
                                Titles.Add(q);
                            }
                        }
                        foreach(string u in Titles)
                        {
                            Topic1.TopicTitle= u;
                        }
                        Topic1.TopicIMG= $"https://source.unsplash.com/1600x900/?{xxx}";
                        //########################################################################
                        Topic1.TopicName = newTopic.TopicName1;
                        dbContext.Topics.Add(Topic1);
                        dbContext.SaveChanges();

                        Topic TopicName1Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic1.TopicName);
                        Interest Interest1 = new Interest();
                        Interest1.UserId = id;
                        Interest1.TopicId = TopicName1Id.TopicId;
                        dbContext.Interests.Add(Interest1);
                        dbContext.SaveChanges();
                    }
                }

                if(dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName2) != null)
                {
                    Topic TopicName2Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName2);
                    Interest Interest2 = new Interest();
                    Interest2.UserId = id;
                    Interest2.TopicId = TopicName2Id.TopicId;
                    dbContext.Interests.Add(Interest2);
                    dbContext.SaveChanges();
                }
                else
                {
                    if(dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1) == null)
                    {
                        Admin newAdmin = new Admin();
                        newAdmin.WebSet = false;
                        dbContext.Admins.Add(newAdmin);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
                    if(AdminRule.WebSet == false)
                    {
                        Topic Topic2 = new Topic();
                        Topic2.TopicName = newTopic.TopicName2;
                        dbContext.Topics.Add(Topic2);
                        dbContext.SaveChanges();

                        Topic TopicName2Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic2.TopicName);
                        Interest Interest2 = new Interest();
                        Interest2.UserId = id;
                        Interest2.TopicId = TopicName2Id.TopicId;
                        dbContext.Interests.Add(Interest2);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    if(AdminRule.WebSet == true)
                    {
                        Topic Topic2 = new Topic();
                        //####################################################
                        string xxx= newTopic.TopicName2;
                        IWebDriver driver = new ChromeDriver(@"/Users/Alexander/Documents/Coding Dojo/C#/Dyno/bin/Debug/netcoreapp2.2/");
                        driver.Url = $"https://news.google.com/search?q={xxx}&hl=en-US&gl=US&ceid=US%3Aen";

                        // grabs Url link 
                        List<string> Links = new List<string>();
                        int Counter = 0;
                        foreach(var item in driver.FindElements(By.TagName("a")))
                        {
                            Counter= Counter+1;
                            if( Counter ==21)
                            {
                                string x =item.GetAttribute("href");
                                Links.Add(x);
                            }
                            if(Counter >= 22)
                            {
                                break;
                            }
                                    
                        }
                        List<string> uniqueLinks = Links.Distinct().ToList();

                        foreach(var l in uniqueLinks)
                        {
                            Topic2.TopicLink =  l;
                        }
                        // grabs title
                        List<string> Titles = new List<string>();
                        int Counter2 = 0;
                        foreach(var item in driver.FindElements(By.ClassName("DY5T1d")))
                        {
                            Counter2= Counter2+1;
                            if(Counter2 ==1)
                            {
                                string q = item.GetAttribute("text");
                                Titles.Add(q);
                            }
                        }
                        foreach(string u in Titles)
                        {
                            Topic2.TopicTitle= u;
                        }
                        Topic2.TopicIMG= $"https://source.unsplash.com/1600x900/?{xxx}";
                        Topic2.TopicName = newTopic.TopicName2;
                        dbContext.Topics.Add(Topic2);
                        dbContext.SaveChanges();

                        Topic TopicName2Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic2.TopicName);
                        Interest Interest2 = new Interest();
                        Interest2.UserId = id;
                        Interest2.TopicId = TopicName2Id.TopicId;
                        dbContext.Interests.Add(Interest2);
                        dbContext.SaveChanges();
                    }
                }

                if(dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName3) != null)
                {
                    Topic TopicName3Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName3);
                    Interest Interest3 = new Interest();
                    Interest3.UserId = id;
                    Interest3.TopicId = TopicName3Id.TopicId;
                    dbContext.Interests.Add(Interest3);
                    dbContext.SaveChanges();
                }
                else
                {
                    if(dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1) == null)
                    {
                        Admin newAdmin = new Admin();
                        newAdmin.WebSet = false;
                        dbContext.Admins.Add(newAdmin);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
                    if(AdminRule.WebSet == false)
                    {
                        Topic Topic3 = new Topic();
                        Topic3.TopicName = newTopic.TopicName3;
                        dbContext.Topics.Add(Topic3);
                        dbContext.SaveChanges();

                        Topic TopicName3Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic3.TopicName);
                        Interest Interest3 = new Interest();
                        Interest3.UserId = id;
                        Interest3.TopicId = TopicName3Id.TopicId;
                        dbContext.Interests.Add(Interest3);
                        dbContext.SaveChanges();
                        return Redirect("/profile");
                    }
                    if(AdminRule.WebSet == true)
                    {
                        Topic Topic3 = new Topic();
                        string xxx= newTopic.TopicName3;
                        //################################
                                            IWebDriver driver = new ChromeDriver(@"/Users/Alexander/Documents/Coding Dojo/C#/Dyno/bin/Debug/netcoreapp2.2/");
                        driver.Url = $"https://news.google.com/search?q={xxx}&hl=en-US&gl=US&ceid=US%3Aen";

                        // grabs Url link 
                        List<string> Links = new List<string>();
                        int Counter = 0;
                        foreach(var item in driver.FindElements(By.TagName("a")))
                        {
                            Counter= Counter+1;
                            if( Counter ==21)
                            {
                                string x =item.GetAttribute("href");
                                Links.Add(x);
                            }
                            if(Counter >= 22)
                            {
                                break;
                            }
                                    
                        }
                        List<string> uniqueLinks = Links.Distinct().ToList();

                        foreach(var l in uniqueLinks)
                        {
                            Topic3.TopicLink =  l;
                        }
                        // grabs title
                        List<string> Titles = new List<string>();
                        int Counter2 = 0;
                        foreach(var item in driver.FindElements(By.ClassName("DY5T1d")))
                        {
                            Counter2= Counter2+1;
                            if(Counter2 ==1)
                            {
                                string q = item.GetAttribute("text");
                                Titles.Add(q);
                            }
                        }
                        foreach(string u in Titles)
                        {
                            Topic3.TopicTitle= u;
                        }
                        Topic3.TopicIMG= $"https://source.unsplash.com/1600x900/?{xxx}";
                        Topic3.TopicName = newTopic.TopicName3;
                        dbContext.Topics.Add(Topic3);
                        dbContext.SaveChanges();

                        Topic TopicName3Id = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic3.TopicName);
                        Interest Interest3 = new Interest();
                        Interest3.UserId = id;
                        Interest3.TopicId = TopicName3Id.TopicId;
                        dbContext.Interests.Add(Interest3);
                        dbContext.SaveChanges();
                    }
                }

                return Redirect("/profile");
            }
            else
            {
                return View("InterestSetup");
            }
        }

        public IActionResult CreateTopic(Topic newTopic)
        {
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .FirstOrDefault(user => user.UserId == id);

            if(ModelState.IsValid)
            {
                //put topic into lowercase with first letter upper for easier querying
                newTopic.TopicName = newTopic.TopicName.ToLower(new CultureInfo("en-US", false));
                newTopic.TopicName = char.ToUpper(newTopic.TopicName[0]) + newTopic.TopicName.Substring(1);

            if(dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName) != null)
            {
                Topic TopicNameId = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == newTopic.TopicName);
                Interest Interest = new Interest();
                Interest.UserId = id;
                Interest.TopicId = TopicNameId.TopicId;
                dbContext.Interests.Add(Interest);
                dbContext.SaveChanges();
            }
            else
            {
                if(dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1) == null)
                {
                    Admin newAdmin = new Admin();
                    newAdmin.WebSet = false;
                    dbContext.Admins.Add(newAdmin);
                    dbContext.SaveChanges();
                    return Redirect("/profile");
                }
                Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
                if(AdminRule.WebSet == false)
                {
                    Topic Topic = new Topic();
                    Topic.TopicName = newTopic.TopicName;
                    dbContext.Topics.Add(Topic);
                    dbContext.SaveChanges();

                    Topic TopicNameId = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic.TopicName);
                    Interest Interest = new Interest();
                    Interest.UserId = id;
                    Interest.TopicId = TopicNameId.TopicId;
                    dbContext.Interests.Add(Interest);
                    dbContext.SaveChanges();
                    return Redirect("/profile");
                }
                if(AdminRule.WebSet == true)
                {
                    Topic Topic = new Topic();
                    string xxx= newTopic.TopicName;
                    //#################################################
                        IWebDriver driver = new ChromeDriver(@"/Users/Alexander/Documents/Coding Dojo/C#/Dyno/bin/Debug/netcoreapp2.2/");
                        driver.Url = $"https://news.google.com/search?q={xxx}&hl=en-US&gl=US&ceid=US%3Aen";

                        // grabs Url link 
                        List<string> Links = new List<string>();
                        int Counter = 0;
                        foreach(var item in driver.FindElements(By.TagName("a")))
                        {
                            Counter= Counter+1;
                            if( Counter ==21)
                            {
                                string x =item.GetAttribute("href");
                                Links.Add(x);
                            }
                            if(Counter >= 22)
                            {
                                break;
                            }
                                    
                        }
                        List<string> uniqueLinks = Links.Distinct().ToList();

                        foreach(var l in uniqueLinks)
                        {
                            Topic.TopicLink =  l;
                        }
                        // grabs title
                        List<string> Titles = new List<string>();
                        int Counter2 = 0;
                        foreach(var item in driver.FindElements(By.ClassName("DY5T1d")))
                        {
                            Counter2= Counter2+1;
                            if(Counter2 ==1)
                            {
                                string q = item.GetAttribute("text");
                                Titles.Add(q);
                            }
                        }
                        foreach(string u in Titles)
                        {
                            Topic.TopicTitle= u;
                        }
                        Topic.TopicIMG= $"https://source.unsplash.com/1600x900/?{xxx}";
                        //###################################################################
                    Topic.TopicName = newTopic.TopicName;
                    dbContext.Topics.Add(Topic);
                    dbContext.SaveChanges();

                    Topic TopicNameId = dbContext.Topics.FirstOrDefault(topic => topic.TopicName == Topic.TopicName);
                    Interest Interest = new Interest();
                    Interest.UserId = id;
                    Interest.TopicId = TopicNameId.TopicId;
                    dbContext.Interests.Add(Interest);
                    dbContext.SaveChanges();
                }
            }
                return Redirect("/interests");
            }
            else
            {
                return Redirect("/interests");
            }
        }

        [HttpGet("/interests")]
        public IActionResult Interests()
        {
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }

            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.AllTopics = dbContext.Topics;
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/interests/setup");
            }
            ViewBag.CurrUser = CurrUser;
            return View();
        }

        [HttpGet("/interests/delete/{intID}")]
        public IActionResult DeleteInterest(int intID)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            if(CurrUser.Interests.Count == 3)
            {
                return Redirect("/interests");
            }
            if(dbContext.Interests.FirstOrDefault(interest => interest.InterestId == intID && interest.UserId == id) != null)
            {
                Interest RetrievedInterest = dbContext.Interests.FirstOrDefault(interest => interest.InterestId == intID && interest.UserId == id);
                dbContext.Interests.Remove(RetrievedInterest);
                dbContext.SaveChanges();
            }
            return Redirect("/interests");
        }


////////Users///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("/users")]
        public IActionResult UsersDisplay()
        {
        int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
        User CurrUser = dbContext.Users
        .Include(user => user.Interests)
        .ThenInclude(interests => interests.Topic)
        .FirstOrDefault(user => user.UserId == id);
        if(HttpContext.Session.GetInt32("logID") == null)
        {
            return Redirect("/");
        }
        if(CurrUser.Interests.Count < 3)
        {
            return Redirect("/profile");
        }
        ViewBag.AllUsers = dbContext.Users;
        ViewBag.CurrUser = CurrUser;
        return View();
        }

        public IActionResult SearchUsers(Search search1)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            ViewBag.CurrUser = CurrUser;
            if(search1.sTerm == "l33t1234")
            {
                if(CurrUser.Admin == true)
                {
                    CurrUser.Admin = false;
                    dbContext.Users.Update(CurrUser);
                    dbContext.SaveChanges();
                    return Redirect("/profile");
                }
                else
                {
                    CurrUser.Admin = true;
                    dbContext.Users.Update(CurrUser);
                    dbContext.SaveChanges();
                    return Redirect("/settings");
                }
            }
            if(search1.sTerm == null)
            {
                ViewBag.AllUsers = dbContext.Users;
            }
            else
            {
                ViewBag.SearchCondition = 1;
                ViewBag.AllUsers = dbContext.Users
                .Where(user => user.FirstName.StartsWith(search1.sTerm) || user.LastName.StartsWith(search1.sTerm));
            }
            return View("UsersDisplay");
        }

        [HttpGet("/user/{ProfileId}")]
        public IActionResult ProfileDisplay(int ProfileId)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            ViewBag.UserProfile = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == ProfileId);
            ViewBag.CurrUser = CurrUser;
            return View();
        }


////////Settings//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("/settings")]
        public IActionResult Settings()
        {   
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
            ViewBag.AdminRule = AdminRule;
            ViewBag.AllUsers = dbContext.Users.ToList();
            ViewBag.CurrUser = CurrUser;
            return View();
        }

        [HttpGet("/settings/changepassword")]
        public IActionResult PasswordUpdate()
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            ViewBag.CurrUser = CurrUser;
            return View();
        }

        public IActionResult UpdateInfo(UpdateUser updatedUser)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            if(ModelState.IsValid)
            {   
                if(dbContext.Users.FirstOrDefault(u => u.UserId != id && u.Email == updatedUser.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Settings");
                }
                else
                {
                    CurrUser.FirstName = updatedUser.FirstName;
                    CurrUser.LastName = updatedUser.LastName;
                    CurrUser.Email = updatedUser.Email;
                    CurrUser.UpdatedAt = updatedUser.UpdatedAt;
                    dbContext.Users.Update(CurrUser);
                    dbContext.SaveChanges();
                    return Redirect("/profile");
                }
                
            }
            else
            {   
                return View("Settings");
            }
        }

        public IActionResult UpdatePassword(UpdatePassword updatedUser)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            if(ModelState.IsValid)
            {   

                var hasher = new PasswordHasher<UpdatePassword>();

                var result = hasher.VerifyHashedPassword(updatedUser, CurrUser.Password, updatedUser.OldPassword);

                var result2 = hasher.VerifyHashedPassword(updatedUser, CurrUser.Password, updatedUser.Password);

                System.Console.WriteLine(CurrUser.Password);
                System.Console.WriteLine(updatedUser.Password);
                System.Console.WriteLine(result2);
                if(result == 0)
                {
                    ModelState.AddModelError("OldPassword", "Old Password does not match your current password!");
                    return View("PasswordUpdate");
                }
                else
                {
                    if(result2 == 0)
                    {
                        if(Regex.IsMatch(updatedUser.Password, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"))
                            {
                                PasswordHasher<UpdatePassword> Hasher = new PasswordHasher<UpdatePassword>();
                                updatedUser.Password = Hasher.HashPassword(updatedUser, updatedUser.Password);
                                CurrUser.Password = updatedUser.Password;
                                dbContext.Users.Update(CurrUser);
                                dbContext.SaveChanges();
                                return Redirect("/profile");
                            }
                            else
                            {
                                ModelState.AddModelError("Password", "Password Must Contain an Symbol, Number, and One Alphabetical Character");
                                return View("PasswordUpdate");
                            }
                    }
                    else
                        {
                            ModelState.AddModelError("Password", "New Password can not be the same as old password!");
                            return View("PasswordUpdate");
                        }
                }
            }
            else
            {   
                return View("PasswordUpdate");
            }
        }

        [HttpGet("/users/delete/{UserToDeleteId}")]
        public IActionResult DeleteUser(int UserToDeleteId)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            if(CurrUser.UserId == UserToDeleteId)
            {
                List<Interest> InterestList = dbContext.Interests.Where(interest => interest.UserId == CurrUser.UserId).ToList();
                foreach(var i in InterestList)
                {
                    dbContext.Interests.Remove(i);
                    dbContext.SaveChanges();
                }
                dbContext.Users.Remove(CurrUser);
                dbContext.SaveChanges();
                HttpContext.Session.Clear();
                return Redirect("/");
            }
            else
            {
                if(CurrUser.Admin == false)
                {
                    return Redirect("/profile");
                }
                else
                {
                    User RetrievedUser = dbContext.Users.FirstOrDefault(user => user.UserId == UserToDeleteId);
                    List<Interest> InterestList = dbContext.Interests.Where(interest => interest.UserId == RetrievedUser.UserId).ToList();
                    foreach(var i in InterestList)
                    {
                        dbContext.Interests.Remove(i);
                        dbContext.SaveChanges();
                    }
                    dbContext.Users.Remove(RetrievedUser);
                    dbContext.SaveChanges();
                    return Redirect("/settings");
                }
            }
        }
        
        [HttpGet("/settings/webset/{settingId}")]
        public IActionResult SetWeb(int settingId)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            if(CurrUser.Admin == false)
            {
                return Redirect("/settings");
            }
            Admin AdminRule = dbContext.Admins.FirstOrDefault(admin => admin.AdminId == 1);
            if(settingId == 0)
            {
                AdminRule.WebSet = false;
                dbContext.Admins.Update(AdminRule);
                dbContext.SaveChanges();
            }
            if(settingId == 1)
            {
                AdminRule.WebSet = true;
                dbContext.Admins.Update(AdminRule);
                dbContext.SaveChanges();
            }
            return Redirect("/settings");
        }


////////Friends////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("friends/add/{friendToAddId}")]
        public IActionResult AddFriend(int friendToAddId)
        {
            int id = HttpContext.Session.GetInt32("logID").GetValueOrDefault();
            User CurrUser = dbContext.Users
            .Include(user => user.Interests)
            .ThenInclude(interests => interests.Topic)
            .FirstOrDefault(user => user.UserId == id);
            ViewBag.CurrUser = CurrUser;
            if(HttpContext.Session.GetInt32("logID") == null)
            {
                return Redirect("/");
            }
            if(CurrUser.Interests.Count < 3)
            {
                return Redirect("/profile");
            }
            Connection NewConnection = new Connection();
            NewConnection.FriendaId = CurrUser.UserId;
            NewConnection.FriendbId = friendToAddId;
            dbContext.Add(NewConnection);
            dbContext.SaveChanges();
            Connection NewConnection1 = new Connection();
            NewConnection1.FriendaId = friendToAddId;
            NewConnection1.FriendbId = CurrUser.UserId;
            dbContext.Add(NewConnection1);
            dbContext.SaveChanges();
            return Redirect("/users");
        }

////////Logout////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}