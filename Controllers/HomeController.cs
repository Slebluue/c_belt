using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
//
using Microsoft.EntityFrameworkCore;
using test.Models;
using System.Linq;
//
using Microsoft.AspNetCore.Identity;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;
 
        public HomeController(Context context)
        {
            _context = context;
        }

        //Current datetime
        public DateTime now()
        {
            DateTime now = DateTime.Now;
            return now;
        }

        public bool CheckLoggedIn()
        {
            int? id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == id);
            ViewBag.User = LoggedIn;
            if(ViewBag.User != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            CheckLoggedIn();
            return View();
        }

        // ------------- LOGIN/REGISTER/LOGOUT --------------//
        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserRegister model)
        {
            if(ModelState.IsValid)
            {
                //Handle success, if any of it failes it hits the catch statement and returns you to the page with email already taken error
                User CheckUser = _context.Users.SingleOrDefault(user => user.email == model.email);
                if(CheckUser != null)
                {
                    ModelState.AddModelError("Email", "Email Address already taken, Please use another");
                    ViewBag.errors = ModelState.Values;
                    return View("Index");
                }

                User Newuser = new User
                {
                    firstname = model.firstname,
                    lastname = model.lastname,
                    email = model.email,
                    password = model.password,
                    createdat = now(),
                    updatedat = now()
                };

                //Hashes and sets new password for registered user
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                string hashed = Hasher.HashPassword(Newuser, Newuser.password);
                Newuser.password = hashed;

                _context.Add(Newuser);
                _context.SaveChanges();
                
                User RetrievedUser = _context.Users.SingleOrDefault(user => user.email == model.email);
                HttpContext.Session.SetInt32("LOGGED_IN_USER", RetrievedUser.userid);
                int? user_id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
                return RedirectToAction("Index", "Some");
            }
            CheckLoggedIn();
            //if form isnt valid
            ViewBag.errors = ModelState.Values;
            return View("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult login(UserLogin model)
        {
            if(ModelState.IsValid)
            {
                //Handle success, if any of it failes it hits the catch statement and returns you to the page with email already taken error
                try 
                {

                    User RetrievedUser = _context.Users.SingleOrDefault(user => user.email == model.LoginEmail);
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();

                    if (0 != Hasher.VerifyHashedPassword(RetrievedUser, RetrievedUser.password, model.LoginPassword))
                    {
                        HttpContext.Session.SetInt32("LOGGED_IN_USER", RetrievedUser.userid);
                        int? user_id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
                        return RedirectToAction("Index", "Some");
                    }
                   else
                   {
                        ModelState.AddModelError("LoginPassword", "Password does not match our records");
                        ViewBag.errors = ModelState.Values;
                        return View("Index");
                   } 
                }
                catch
                {
                    //this should only catch if email is already taken. Other fields validated
                    ModelState.AddModelError("LoginEmail", "Email not valid");
                    ViewBag.errors = ModelState.Values;
                    return View("Index");
                }
                
            }
            //if form isnt valid
            CheckLoggedIn();
            ViewBag.errors = ModelState.Values;
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
