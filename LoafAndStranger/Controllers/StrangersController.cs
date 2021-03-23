using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.DataAccess;
using LoafAndStranger.Models;

namespace LoafAndStranger.Controllers
{
    [Route("api/Strangers")]
    [ApiController]
    public class StrangersController : ControllerBase
    {
        StrangerRepository _repo;
        public StrangersController(StrangerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetAllStrangers()
        {
            return Ok(_repo.GetAll());
        }
    }
}
