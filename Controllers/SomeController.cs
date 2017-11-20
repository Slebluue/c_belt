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
    public class SomeController : Controller
    {
        private Context _context;
 
        public SomeController(Context context)
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
        [Route("/profile")]
        public IActionResult Index()
        {
            if(!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            int? id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == id);
            List<Invite> UserInvites = _context.Invites.Include(u => u.sender).Where(i => i.userid == LoggedIn.userid).ToList();
            List<Friendship> UserFriends = _context.Friends.Include(u => u.friend).Where(i => i.userid == LoggedIn.userid).ToList();
            ProfileWrapper model = new ProfileWrapper(LoggedIn, UserInvites, UserFriends);
            return View(model);
        }
        [HttpPost]
        [Route("/updateprofile")]
        public IActionResult UpdateProfile(string desc)
        {
            int? id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == id);
            LoggedIn.desc = desc;
            _context.SaveChanges();
            return RedirectToAction("Index", "Some");
        }
        [HttpGet]
        [Route("/users")]
        public IActionResult Users()
        {
            if(!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            int? id = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            List<User> AllUsers = _context.Users.Include(c => c.friends).Where(u => u.userid != (int)id).ToList();
            List<Friendship> Connections = _context.Friends.Where(c => c.userid == (int)id).ToList();
            List<User> Updated = new List<User>();
            foreach(var user in AllUsers)
            {   
                var connections = Connections as List<Friendship>;
                if(connections.Any(c => c.friendid == user.userid) )
                {
                    Console.WriteLine("Pass");
                }
                else
                {
                    Updated.Add(user);
                }
                
            }
            ViewBag.Connections = Connections;
            UserWrapper model = new UserWrapper(Updated, Connections);
            return View(model);
        }
        [HttpGet]
        [Route("/users/{id}")]
        public IActionResult GetUser(int id)
        {
            if(!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            User user = _context.Users.SingleOrDefault(u => u.userid == id);
            return View(user);
        }
        [HttpGet]
        [Route("/connect/{id}")]
        public IActionResult Connect(int id)
        {
            
            int? LoggedID = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            bool something = _context.Invites.Where(i => i.userid == id && i.senderid == LoggedID).Any();
            if(something)
            {
                return RedirectToAction("Users");
            }
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == LoggedID);
            Invite NewInvite = new Invite(){
                senderid = (int)LoggedID,
                sender = LoggedIn,
                userid = id
            };
            _context.Add(NewInvite);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }
        [HttpGet]
        [Route("/remove/{SenderID}")]
        public IActionResult Remove(int SenderID)
        {
            int? LoggedID = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == LoggedID);
            User Sender = _context.Users.SingleOrDefault(u => u.userid == SenderID);
           
            Friendship AddToUser = _context.Friends.SingleOrDefault(f => f.userid == LoggedIn.userid && f.friendid ==Sender.userid);
               
            Friendship AddToFriend = _context.Friends.SingleOrDefault(f => f.userid == Sender.userid && f.friendid ==LoggedIn.userid); 

            LoggedIn.friends.Remove(AddToFriend);
            Sender.friends.Remove(AddToUser); 

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("/accept/{SenderID}")]
        public IActionResult Accept(int SenderID)
        {
            int? LoggedID = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == LoggedID);
            User Sender = _context.Users.SingleOrDefault(u => u.userid == SenderID);

            Friendship AddToUser = new Friendship(){
                userid = LoggedIn.userid,
                friendid = Sender.userid,
                friend = Sender,
            };
            Friendship AddToFriend = new Friendship(){
                userid = Sender.userid,
                friend = LoggedIn,
                friendid = LoggedIn.userid,
            };
            LoggedIn.friends.Add(AddToFriend);
            Sender.friends.Add(AddToUser);

            Invite ToRemove = _context.Invites.SingleOrDefault(i => i.userid == LoggedID && i.senderid == SenderID);
            _context.Invites.Remove(ToRemove);            
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("/ignore/{SenderID}")]
        public IActionResult Ignore(int SenderID)
        {
            int? LoggedID = HttpContext.Session.GetInt32("LOGGED_IN_USER");
            User LoggedIn = _context.Users.SingleOrDefault(user => user.userid == LoggedID);
            User Sender = _context.Users.SingleOrDefault(u => u.userid == SenderID);
            Invite ToRemove = _context.Invites.SingleOrDefault(i => i.userid == LoggedID && i.senderid == SenderID);
            _context.Invites.Remove(ToRemove);            
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
