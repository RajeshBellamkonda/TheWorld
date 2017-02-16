using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;

        public AppController(IMailService mailService, IConfigurationRoot config)
        {
            _mailService = mailService;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel contactVm)
        {
            if (contactVm.Email.Contains("test.com"))
                ModelState.AddModelError("Email", "It's a fake address dude");
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], contactVm.Email, "From TheWorld", contactVm.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";

            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
