using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.DataAccess;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class AdminController : ControllerBase
    {
        private SqlServerDataAccess _dataAccess;
        private TokenHelper _token;

        public AdminController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config);
            _token = new TokenHelper(config);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AdminModel model)
        {
            int adminId = _dataAccess.IsAdminLoginValid(model.Name, model.HashedPw);
            if (adminId == 0)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new { token = _token.GenerateJSONWebToken(adminId,true)});
            }
        }
        /// <summary>
        /// create new election with information of election description and candidates information 
        /// (cannot be updated after creation)
        /// (you can add voters after creation)
        /// </summary>
        [HttpPut]
        public IActionResult CreateNewElection()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userIdFromToken = Convert.ToInt32(claim[0].Value);

            return Ok();
        }

        [HttpPost]
        public IActionResult CompleteTheElection()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userIdFromToken = Convert.ToInt32(claim[0].Value);

            return Ok();
        }
        /// <summary>
        /// send list of voters email for the specified election
        /// email and hashed password will automatically send to added voter manually
        /// </summary>
        [HttpPut]
        public IActionResult AddVotersToElection()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userIdFromToken = Convert.ToInt32(claim[0].Value);

            return Ok();
        }

        /// <summary>
        /// delete specified voter from the election
        /// </summary>
        [HttpDelete]
        public IActionResult DeleteVoterFromElection()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userIdFromToken = Convert.ToInt32(claim[0].Value);

            return Ok();
        }



    }
}
