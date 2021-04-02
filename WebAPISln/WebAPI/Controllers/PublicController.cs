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
using WebAPI.Models;

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
            return Ok(_dataAccess.GetAllElections());
        }
        [HttpGet]
        public IActionResult GetCandidatesOfElection([FromQuery] int electionId)
        {
            return Ok(_dataAccess.GetCandidatesOfElection(electionId));
        }

        [HttpGet]
        public IActionResult GetElectionResultsIfCompleted([FromQuery] int electionId)
        {
            List<string> decryptedVotes = new List<string>();
            if (_dataAccess.GetElectionDetailsFromId(electionId).IsCompleted == true)
            {
                List<string> votes = _dataAccess.GetVotesOfElection(electionId);
                foreach (string vote in votes)
                {
                    decryptedVotes.Add(RSAHelper.DecryptRSA(vote, Keys.PrivKey, "utf8"));
                }
            }
            return Ok(decryptedVotes);
        }


    }
}
