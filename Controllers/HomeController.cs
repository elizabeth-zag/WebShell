using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebShell.Models;
using WebShell.Services;

namespace WebShell.Controllers
{
    public class HomeController : Controller
    {
        ProcessingService processingService;
        public HomeController(ProcessingService service)
        {
            processingService = service;
        }
        public IActionResult Index()
        {
            IEnumerable<Request> requests = processingService.GetRequests();
            return View(requests);
        }

        [HttpPost]
        public ActionResult ProcessRequest(Request request)
        {
            ViewBag.Message = processingService.ProcessRequest(request);
            return View();

        }
    }
}
