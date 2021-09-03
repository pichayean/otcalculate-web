using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using otcalculate.Services;
using OTCalculate.Extension;
using OTCalculate.Models;
using OTCalculate.Process;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace OTCalculate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly IDataService _dataService;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment HostEnvironment, IDataService dataService)
        {
            _logger = logger;
            _HostEnvironment = HostEnvironment;
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            var datas = _dataService.GetData();
            if (!String.IsNullOrEmpty(datas))
            {
                var data = JsonSerializer.Deserialize<List<Employee>>(datas);
                ViewBag.OT = data;
                ViewBag.OTTOTAL = data.ToTotalOT();
                Console.WriteLine("Done");
            }
            else
            {
                ViewBag.OT = null;//new List<Employee>();
                ViewBag.OTTOTAL = null;
            }
            Console.WriteLine("View Index");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult OT(IFormFile image)
        {
            if (image != null)
            {
                try
                {
                    var data = LexicalParser.ToEmpolyee(image);
                    var d = JsonSerializer.Serialize(data);
                    // TempData["OT"] = JsonSerializer.Serialize(data);
                    _dataService.SetData(d);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DownloadTemlpate()
        {
            Console.WriteLine("test");
            string file = @"template1.xlsx";
            string webRootPath = _HostEnvironment.WebRootPath;
            string contentRootPath = _HostEnvironment.ContentRootPath;

            string path = "";
            path = Path.Combine(webRootPath, "template1.xlsx");
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(path));
        }

    }
}
