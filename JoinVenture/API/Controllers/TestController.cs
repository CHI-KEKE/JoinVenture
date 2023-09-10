using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController:BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Testing()
        {
            Console.WriteLine("Testing K6!!");
            return Ok("K6 testing!!");
        }        
    }
}