using CityInfo.Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Route("api/testdatabase")]
    public class DummyController:ControllerBase
    {
        private readonly CityInfoContext _ctx;
        public DummyController(CityInfoContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
