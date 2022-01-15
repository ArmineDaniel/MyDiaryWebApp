using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyDiaryWebApp.ViewModels; 
using MyDiaryWebApp.Models; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyDiaryWebApp.Controllers
{
    public class AccountController: Controller
    {
        private UsersContext db;
        public AccountController(UsersContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email); // authentication
                    db.Entry(user).GetDatabaseValues();
                    ThisUserId.thisId = user.Id;

                    return RedirectToAction("Note", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and (or) password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // add the user to the database
                    var newUser = new User { Email = model.Email, Password = model.Password };
                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email); // authentication
   
                    ThisUserId.thisId = newUser.Id;
                    return RedirectToAction("Note", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login and (or) password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Note()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Note(NoteModel model)
        {
            if (ModelState.IsValid)
            {

                Diaries diary = await db.Diaries.FirstOrDefaultAsync(d => d.DateOfDiary == model.DateOfDiary && d.UserId==ThisUserId.thisId);
                ThisUserId.thisDateOfDiary = model.DateOfDiary;
                if (diary == null)
                {
                    await Authenticate(model.DateOfDiary.ToString()); // authentication

                    return RedirectToAction("Diary", "Account");
                }
                
                else
                {
                   
                    await Authenticate(model.DateOfDiary.ToString()); // authentication
                    return RedirectToAction("DiaryOfBase", "Account");
                }
            }
            return View(model);
            }

         public async Task<IActionResult> DiaryOfBase(NoteModel model)
        {

            return View(await db.Diaries.Where(c => c.DateOfDiary == ThisUserId.thisDateOfDiary && c.UserId== ThisUserId.thisId).ToListAsync());
            
        }

        public async Task<IActionResult> RemoveDiary()
        {
            if (ModelState.IsValid)
            {
                //Remove diary
                Diaries diary = await db.Diaries.Where(c => c.DateOfDiary == ThisUserId.thisDateOfDiary && c.UserId == ThisUserId.thisId).FirstOrDefaultAsync();
                db.Diaries.Remove(diary);
                await db.SaveChangesAsync();

            }
            return RedirectToAction("Note", "Home");
        }


        //public async Task<IActionResult> EditDiary()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Diaries diary = await db.Diaries.Where(c => c.DateOfDiary == ThisUserId.thisDateOfDiary && c.UserId == ThisUserId.thisId).FirstOrDefaultAsync();
                
        //        NewText.NewTextOfDiary= diary.TextOfDiary;
        //        db.Entry(diary).State = EntityState.Modified;
        //        //Diaries newdiary = await db.Diaries.FirstOrDefaultAsync(d => d.DateOfDiary == ThisUserId.thisDateOfDiary && d.UserId == ThisUserId.thisId && d.TextOfDiary == NewText.NewTextOfDiary);
        //        await db.SaveChangesAsync();

        //    }
        //    return RedirectToAction("Note", "Home");
        //}


        [HttpGet]
        public IActionResult Diary()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Diary(DiaryModel model)
        {
            if (ModelState.IsValid)
            {
                Diaries diary = await db.Diaries.FirstOrDefaultAsync(d => d.DateOfDiary == ThisUserId.thisDateOfDiary && d.UserId == ThisUserId.thisId);
                if (diary == null)
                {
                    // add the new diary to the database
                    db.Diaries.Add(new Diaries { DateOfDiary = ThisUserId.thisDateOfDiary, TextOfDiary=model.TextOfDiary, UserId= ThisUserId.thisId });
                    await db.SaveChangesAsync();

                    return RedirectToAction("Note", "Home");
                }
            }
            return View(model);
        }




        private async Task Authenticate(string userName)
        {
            // create one claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // create a ClaimsIdentity object
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // setting authentication cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
