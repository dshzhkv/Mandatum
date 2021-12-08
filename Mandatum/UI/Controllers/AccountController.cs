﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Mandatum.Convertors;
using Mandatum.Models;
using Mandatum.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Mandatum.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserApi _userApi;
        private readonly UserConvertorModel _convertorModel;
        private readonly UserConvertorRegister _convertorRegister;
        private readonly UserManager<UserRecord> _userManager;
        private readonly SignInManager<UserRecord> _signInManager;

        public AccountController(UserManager<UserRecord> userManager, SignInManager<UserRecord> signInManager, UserApi userApi, UserConvertorModel convertorModel, UserConvertorRegister convertorRegister)
        {
            _userApi = userApi;
            _convertorModel = convertorModel;
            _convertorRegister = convertorRegister;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        
        public async Task<IActionResult> GoogleResponse()
        {
            
            var response = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var email = response.Principal?.Identities.FirstOrDefault()
                .Claims
                .Where(claim => (claim.Type.Split("/").Last() =="emailaddress" ))
                .Select(claim => claim.Value.Split("/").Last())
                .FirstOrDefault();
            var username = response.Principal.Identities.FirstOrDefault()?
                .Claims.Where(claim => (claim.Type.Split("/").Last() =="givenname" ))
                .Select(claim => claim.Value.Split("/").Last())
                .FirstOrDefault();
            
            var user = new UserRecord() {Email = email, UserName = username};
            //тут надо сделать штуку, чтобы спрашивать у юзеров пароль, пока все хранятся с одинаковым
            var result = await _userManager.CreateAsync(user, "Qwer1%");
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            
            return RedirectToAction("Index", "Home");
            
        }
     
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            
            return View(new LoginModel { ReturnUrl = returnUrl });
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
 
        
        public async Task<IActionResult> Logout()
        {
            
            await _signInManager.SignOutAsync();
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserRecord() {Email = model.Email, UserName = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}