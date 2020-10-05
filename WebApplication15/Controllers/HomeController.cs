using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Microsoft.Extensions.Logging;
using WebApplication15.Entities;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private SendgridwebhookdbContext context;

        public HomeController(ILogger<HomeController> logger, SendgridwebhookdbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetData()
        {
            int start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string searchValue = HttpContext.Request.Form["search[value]"];
            string sortColumnName = HttpContext.Request.Form["columns[" + Request.Form["order[0][column]"] + "][name]"];
            string sortDirection = HttpContext.Request.Form["order[0][dir]"];

            List<TestDatas> empList = new List<TestDatas>();


            using (context)
            {
                empList = await context.TestDatas.ToListAsync();
                int totalrows = empList.Count;
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    empList =   empList.
                        Where(x => x.Email.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                int totalrowsafterfiltering = empList.Count;
                //sorting

                try
                {
                    empList = empList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();

                }
                catch (Exception ex)
                {

                    throw;
                }


                //paging
                empList = empList.Skip(start).Take(length).ToList<TestDatas>();


                return Json(new { data = empList, draw = Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering });
            }





        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
