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

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Voter")]
    public class VoterController : ControllerBase
    {
        private SqlServerDataAccess _dataAccess;
        private TokenHelper _token;

        public VoterController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config);
            _token = new TokenHelper(config);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login()
        {
            //int voterId = _dataAccess.IsVoterLoginValid(email, HashedPw);
            //if (voterId == 0)
            //{
            //    return Unauthorized();
            //}
            //else
            //{
            //    return Ok(new { token = _token.GenerateJSONWebToken(adminId, false) });
            //}
            return Ok(new { token = _token.GenerateJSONWebToken(123, false) });
        }

        [HttpPut]
        public IActionResult CreateNewElection()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userIdFromToken = Convert.ToInt32(claim[0].Value);

            return Ok();
        }
    }
}
