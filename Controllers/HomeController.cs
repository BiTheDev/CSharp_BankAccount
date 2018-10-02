using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private BankContext _context;
        public HomeController(BankContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View("index");
        }

        [HttpPost("RegisterProcess")]
        public IActionResult Register(RegisterViewModel user){
                if(ModelState.IsValid){
                PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                user.password = Hasher.HashPassword(user, user.password);
                Users User = new Users(){
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                // List<Users> Users = _context.users.Include(p => p.accounts).ToList();
                _context.Add(User);
                _context.SaveChanges();
                return RedirectToAction("Status");
            }else{
                return View("index");
            }
        }


        [HttpGet("Login")]
        public IActionResult LoginPage(){

            return View("login");
        }

        [HttpPost("LoginProcess")]
        public IActionResult Login(LoginViewModel User){
            if(ModelState.IsValid){
                List<Users> users = _context.users.Where(p => p.email== User.Email).ToList();
                foreach (var user in users)
                {
                    if(user != null && User.Password != null)
                        {
                            var Hasher = new PasswordHasher<Users>();
                            if( 0 !=Hasher.VerifyHashedPassword(user, user.password, User.Password)){
                                HttpContext.Session.SetInt32("Id", (int)user.id);
                                int? id = HttpContext.Session.GetInt32("Id");

                            return RedirectToAction("Status", new{id = id});
                        }
                    }else{
                        return View("login");
                    }
                }       
            }
            return View("login");
        }

        [HttpGet("Status/{id}")]
        public IActionResult Status(){
            int? id = HttpContext.Session.GetInt32("Id");
            double UserAccount = _context.accounts.Where(p => p.userid == (int)id).Sum(p =>p.change);
            List<accounts> ListTrans = _context.accounts.Where(p => p.userid == (int)id).OrderByDescending(p =>p.created_at).ToList();
            IEnumerable<Users> User = _context.users.Where(p => p.id == (int)id);
            System.Console.WriteLine(id);
            ViewBag.user = User;
            ViewBag.accounts = UserAccount;
            ViewBag.List  = ListTrans;
            return View("account");
        }


        [HttpPost("ProcessMoney")]
        public IActionResult Process(double change){
            int? id = HttpContext.Session.GetInt32("Id");
            accounts UserAccount = _context.accounts.Where(p => p.userid == (int)id).FirstOrDefault();   
            if(UserAccount == null){
            accounts newaccount = new accounts(){
                userid = (long)id,
                change = change,    
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _context.Add(newaccount);
            _context.SaveChanges();
            }
            else{
                accounts newaccount = new accounts(){
                    userid = (long)id,
                    change = change,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                _context.Add(newaccount);
                _context.SaveChanges();
            }      
            return RedirectToAction("Status", new{id = id});
        }

        [HttpPost("logout")]
        public IActionResult logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
