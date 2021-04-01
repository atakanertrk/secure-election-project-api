using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataAccess;
using WebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublicController : ControllerBase
    {

        private SqlServerDataAccess _dataAccess;
        private TokenHelper _token;

        public PublicController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config);
            _token = new TokenHelper(config);
        }

        [HttpGet]
        public IActionResult GetListOfElections()
        {
            return Ok();
        }


    }
}
