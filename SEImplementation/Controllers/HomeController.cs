﻿using System.Web.Mvc;
using System.Web.Security;
using Common;
using BusinessLayer;
using DesignPattern;

namespace SEImplementation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Order OrderState = new Order();
            string x = OrderState.Register();
            
            //TODO : ORDER STATE
            ViewBag.lblOrderState = x;
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult Register()
        {
            return View(new Models.RegisterModel());
        }

        [HttpPost]
        public ActionResult Register(Models.RegisterModel rm)
        {
            if (new UserBL().DoesEmailExist(rm.UserName) && new UserBL().DoesUserNameExist(rm.UserName)) { return Redirect("/home/register?msg=userandemailtaken"); }
            else if (new UserBL().DoesEmailExist(rm.UserName)) { return Redirect("/home/register?msg=emailtaken"); }
            else if (new UserBL().DoesUserNameExist(rm.UserName)) { return Redirect("/home/register?msg=usernametaken"); }
            else
            {
                User u = new User();
                u.Username = rm.UserName;
                u.FirstName = rm.FirstName;
                u.Surname = rm.Surname;
                u.Email = rm.Email;
                u.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(rm.Password, "MD5");
                u.MobileNum = rm.MobileNum;

                new UserBL().CreateUser(u);
                return Redirect("/?msg=success");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        //to post details


        [HttpPost]
        public ActionResult Login(Models.LoginModel lm)
        {
            if (new UserBL().DoesUserNameExist(lm.username))
            {
                User u = new UserBL().GetUserByUsername(lm.username);
                try
                {
                    string password = FormsAuthentication.HashPasswordForStoringInConfigFile(lm.password, "MD5");
                    if (new UserBL().isAuthenticated(lm.username, password))
                    {
                        FormsAuthentication.RedirectFromLoginPage(lm.username, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View();
                    }
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
