using System;
using System.Collections.Generic;
using System.Linq;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Controllers 
{
    public class HomeController : Controller 
    {
        private HomeContext dbContext;
        public HomeController (HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost("newUser")]
        public IActionResult Create(User myUser)
        {
            if(ModelState.IsValid) 
            {   
                if(dbContext.Users.Any(u => u.Email == myUser.Email))
                {   
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                } 
                else
                {   
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    myUser.Password = Hasher.HashPassword(myUser, myUser.Password);
                    dbContext.Users.Add(myUser);
                    dbContext.SaveChanges();
                    return RedirectToAction("Login");    
                }
                    
            }
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }  

        [HttpPost("loginprocess")]
        public IActionResult LoginProcess(LoginUser thisUser)
        { 
            if(ModelState.IsValid)
                {
                    var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == thisUser.Email);
                
                    if(userInDb == null)
                    {
                        ModelState.AddModelError("Email", "Invalid Email or Password");
                        return View("Login");
                    }   
                    
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(thisUser, userInDb.Password, thisUser.Password);
                    
                    if(result == 0)
                    {
                        ModelState.AddModelError("Email", "Invalid Email or Password");
                        return View("Login");
                    }
                    else
                    {
                        HttpContext.Session.SetString("Email", thisUser.Email);
                        string UserEmail = HttpContext.Session.GetString("Email");
                        return RedirectToAction("Dashboard");          
                    }
                }
                return View("Dashboard");
            
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User user = dbContext.Users.FirstOrDefault(u=>u.Email == HttpContext.Session.GetString("Email"));
            ViewBag.user = user;
    
            if (user == null)
            {   
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }
            
            List<Game> allGames = dbContext.Games.Include(u=>u.Participants).Include(u=>u.Creator).Include(u=>u.Participants).OrderByDescending(u=>u.GameDate).ToList();
            return View(allGames);        
        }

        [HttpGet("newGame")]
        public IActionResult NewGame()
        {
            return View();
        }

        [HttpPost("create/game")]
        public IActionResult Create(Game newGame)
        {
            if(ModelState.IsValid)
            {
                User currentUser= dbContext.Users.FirstOrDefault(u=>u.Email==HttpContext.Session.GetString("Email"));
                
                newGame.Creator = currentUser;
                newGame.UserId = currentUser.UserId;
                
                dbContext.Games.Add(newGame);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewGame");
        }

        [HttpGet("game/{GameId}")]
        public IActionResult Game(int GameId)
        {   
            User user = dbContext.Users.FirstOrDefault( u => u.Email == HttpContext.Session.GetString("Email"));

            Game showGame = new Game();
            showGame = dbContext.Games.Include(u=>u.Creator).Include(u=>u.Participants).ThenInclude(u=>u.User).FirstOrDefault(u=>u.GameId==GameId);
            return View(showGame);
        }


        [HttpGet("join/game/{GameId}")]
        public IActionResult JoinGame(int GameId)
        {    
            User user = dbContext.Users.FirstOrDefault(u => u.Email == HttpContext.Session.GetString("Email"));
            
            Game game = dbContext.Games.FirstOrDefault(g => g.GameId == GameId);

            Association joinGame = new Association();
                joinGame.UserId = user.UserId;
                joinGame.GameId = game.GameId;

            
            dbContext.Associations.Add(joinGame);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");

        } 

        [HttpGet("leave/game/{GameId}")]
        public IActionResult LeaveGame(int GameId)
        {
            User user = dbContext.Users.FirstOrDefault( u => u.Email == HttpContext.Session.GetString("Email"));

            Game game = dbContext.Games.FirstOrDefault(g => g.GameId == GameId);


            Association leaveGame = dbContext.Associations.FirstOrDefault(u=>u.UserId==user.UserId && u.GameId==game.GameId);
                

            dbContext.Associations.Remove(leaveGame);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        
        [HttpGet("delete/game/{GameId}")]
        public IActionResult DeleteGame(int GameId)
        {
            if(HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToAction("Index");
            }
            
            Game deleteGame = new Game();
            deleteGame = dbContext.Games.FirstOrDefault( a => a.GameId == GameId);
            dbContext.Games.Remove(deleteGame);
            dbContext.SaveChanges();
                
            return RedirectToAction("dashboard");
        }
        
        [HttpGet("edit/game/{GameId}")]
        public IActionResult EditGame(int GameId)
        {
            Game editGame = dbContext.Games.FirstOrDefault( g => g.GameId == GameId);
            return View(editGame);
        }

        [HttpPost("update/game/{GameId}")]
        public IActionResult UpdateGame(Game updateGame, int GameId)
        {
            if(ModelState.IsValid)
            {   
                User user = dbContext.Users.FirstOrDefault( u => u.Email == HttpContext.Session.GetString("Email"));
                
                Game Update = dbContext.Games.FirstOrDefault(d => d.GameId == GameId);
                Update.Title = updateGame.Title;
                Update.GameDate = updateGame.GameDate;
                Update.Time = updateGame.Time;
                Update.Description = updateGame.Description;
                Update.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("dashboard");
            }

            return View("EditGame");
        }


        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }   
    
    }    
            
}   