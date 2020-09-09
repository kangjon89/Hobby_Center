using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private int? UserSessionId
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private string UserSessionName
        {
            get { return HttpContext.Session.GetString("UserName"); }
            set { HttpContext.Session.SetString("UserName", value); }
        }
        private MyContext dbContext;
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Entrance()
        {
            return View();
        }

        public IActionResult Register(User newUser){

            if(ModelState.IsValid){
                var duplicateCheck = dbContext.Users
                    .FirstOrDefault(u=>u.Email==newUser.Email);
                if(duplicateCheck !=null){
                    ModelState.AddModelError("Email","This email is already in use !");
                    return View("Entrance");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(newUser,newUser.Password);
                newUser.Password = hashedPw;
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                UserSessionName = newUser.Name;
                UserSessionId = newUser.UserId;
                return Redirect("/Dashboard");
            }
            return View("Entrance");
        }

        [HttpPost("userLogin")]
        public IActionResult userLogin(LoginUser currUser){
            if(ModelState.IsValid){
                User existingUser = dbContext.Users
                    .FirstOrDefault(u => u.Email == currUser.Email);
                if (existingUser == null){
                    ModelState.AddModelError("Email","Please register first");
                    return View("Entrance");
                }
                else{
                    PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(currUser, existingUser.Password, currUser.Password);
                    if(result ==0){
                        ModelState.AddModelError("Email","Invalid Password");
                        return View("Entrance");
                    }
                    UserSessionName = existingUser.Name;
                    UserSessionId = existingUser.UserId;
                    return Redirect("/Dashboard");
                }
                
            }
            return View("Entrance");

        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard(){
            if (UserSessionId !=null){
                ViewBag.AllSports = dbContext.Sports
                .Include(j=>j.Guests)
                .ThenInclude(u=>u.User)
                .ToList();
                List <Sport>AllSports = dbContext.Sports
                    .Include(w=>w.Guests)
                    .ToList();
                ViewBag.CurrUserId = UserSessionId;
                ViewBag.CurrUserName = UserSessionName;
                
                
                return View(AllSports);
            }
            return RedirectToAction("Entrance");
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Entrance");
        }
        [HttpGet("NewHobby")]
        public IActionResult NewSport(){
            return View();
        }

        [HttpPost("/AddHobby")]
        public IActionResult AddSport(Sport newSport){
            
            if(ModelState.IsValid){
                newSport.CreatorId = (int)UserSessionId;    
                newSport.CreatorName = UserSessionName;
                dbContext.Sports.Add(newSport);
                dbContext.SaveChanges();
                return RedirectToAction("ShowSport",new{SportId = newSport.SportId});
            }
            return View("NewSport");
        }
        [HttpGet("ShowSport/{SportId}")]
        public IActionResult ShowSport(int SportId){
            ViewBag.OneSport = dbContext.Sports        
                .Include(g=>g.Guests)
                .ThenInclude(u=>u.User)
                .FirstOrDefault(w=>w.SportId==SportId);
            ViewBag.CurrUserId = UserSessionId;
            var OneSport = dbContext.Sports.
            FirstOrDefault(s=>s.SportId == SportId);
            return View(OneSport);
        }

        [HttpGet("delete/{SportId}")]
        public IActionResult Delete(int SportId){
            var Todelete = dbContext.Sports
                .FirstOrDefault(s=>s.SportId == SportId);
            dbContext.Sports.Remove(Todelete);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("join/{SportId}")]
        public IActionResult Join(int SportId){
            Join newJoin = new Join(){
                SportId = SportId,
                UserId = (int)UserSessionId
            };
            dbContext.Joins.Add(newJoin);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        }
        
        [HttpGet("leave/{SportId}")]
        public IActionResult Leave(int SportId){
            var ToLeave = dbContext.Joins
                .FirstOrDefault(j=>j.SportId == SportId && j.UserId == (int)UserSessionId);
            dbContext.Joins.Remove(ToLeave);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        }
    }
}