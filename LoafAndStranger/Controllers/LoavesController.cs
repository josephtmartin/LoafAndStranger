using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using LoafAndStranger.DataAccess;

namespace LoafAndStranger.Controllers
{
    [Route("api/Loaves")]
    [ApiController]
    public class LoavesController : ControllerBase
    {
        LoafRepository _repo;
        public LoavesController()
        {
            _repo = new LoafRepository();
        }

        //GET to /api/loaves
        [HttpGet]
        public IActionResult GetAllLoaves()
        {
            return Ok(_repo.GetAll());
        }

        //POST to /api/loaves
        [HttpPost]
        public IActionResult AddALoaf(Loaf loaf)
        {
            _repo.Add(loaf);
            return Created($"api/Loaves/{loaf.Id}", loaf);
        }

        //GET to /api/loaves/{id}
        //GET to /api/loaves/4
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var loaf = _repo.Get(id);
            if (loaf == null)
            {
                return NotFound("This loaf id does not exist");
            }
            return Ok(loaf);
        }

        //Idempotency -> Idempotent
        //PUT to /api/loaves/{id}/slice
        //PUT to /api/loaves/4/slice
        [HttpPut("{id}")]
        public IActionResult SliceLoaf(int id)
        {
            var loaf = _repo.Get(id);

            if (loaf.Sliced)
            {
                return NoContent();
            }

            loaf.Sliced = true;
        }




    }
}