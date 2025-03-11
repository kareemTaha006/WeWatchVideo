using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeWatchVideo.Models;

namespace WeWatchVideo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const int VideoDuration = 13; 

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WatchVideo()
        {
            return View();
        }

   

        [HttpPost]
        public void StartVideo()
        {
            SetStartTimeSession();

        }

        [HttpPost]
        public IActionResult EndVideo()
        {
            DateTime endTime = DateTime.Now;
            var startTimeStr = HttpContext.Session.GetString("StartTime_userId");
            if (startTimeStr == null)
            {
                return Json(new { restart = true });
            }

            DateTime.TryParse(startTimeStr, out DateTime startTime);
            double watchedTime = (endTime - startTime).TotalSeconds;

            if (watchedTime < VideoDuration)
            {
                SetStartTimeSession();
                return Json(new { restart = true });
            }

            SetStartTimeSession();
            var scripttxt = GetEventScript();
            return Json(new { scripttxt = scripttxt });
        }

        private void SetStartTimeSession()
        {
            HttpContext.Session.SetString("StartTime_userId", DateTime.Now.ToString("o"));//todo add user id 
        }

        private string GetEventScript()
        {

            return @" 
                    const btn = document.createElement('button');
                    btn.id = 'subscribeBtn';
                    btn.textContent = 'Subscribe';
                    btn.style.padding = '10px 20px';
                    btn.style.backgroundColor = 'red';
                    btn.style.color = 'white';
                    btn.style.border = 'none';
                    btn.style.cursor = 'pointer';
                    btn.style.fontSize = '16px';
                    btn.style.marginTop = '10px';
                    btn.style.display = 'block';

                    btn.addEventListener('click', function () {
                        alert('Thank you for subscribing!');
                    });

                    document.body.appendChild(btn);
                    
       ";
        }
    }

}

