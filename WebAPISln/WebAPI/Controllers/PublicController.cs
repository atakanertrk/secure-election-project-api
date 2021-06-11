using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessClassLibrary.DataAccess;
using WebAPI.Helpers;
using WebAPI.Models;
using DataAccessClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PublicController : ControllerBase
    {

        private IDataAccess _dataAccess;
        private TokenHelper _token;

        public PublicController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config.GetConnectionString("SqlServerConnectionString"));
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
            if (_dataAccess.GetElectionDetailsFromId(electionId).IsCompleted == true)
            {
                List<string> decryptedVotes = new List<string>();
                List<string> votes = _dataAccess.GetVotesOfElection(electionId);
                foreach (string vote in votes)
                {
                    decryptedVotes.Add(RSAHelper.DecryptRSA(vote, Keys.PrivKey, "utf8"));
                }
                List<ElectionResultsModel> resultList = new List<ElectionResultsModel>();
                foreach (var candidate in _dataAccess.GetCandidatesOfElection(electionId))
                {
                    ElectionResultsModel result = new ElectionResultsModel();
                    result.CandidateId = candidate.Id;
                    result.CandidateName = candidate.Name;
                    result.Votes = decryptedVotes.Where(x => x == candidate.Id.ToString()).ToList().Count();
                    resultList.Add(result);
                }
                return Ok(resultList);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
