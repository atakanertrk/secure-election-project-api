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
    //[Authorize(AuthenticationSchemes = "Voter")]
    public class VoterController : ControllerBase
    {
        private SqlServerDataAccess _dataAccess;
        private TokenHelper _token;

        public VoterController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config);
            _token = new TokenHelper(config);
        }

        //private int GetIdFromToken()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IList<Claim> claim = identity.Claims.ToList();
        //    return Convert.ToInt32(claim[0].Value);
        //}
        [HttpPut]
        public IActionResult Vote([FromBody] VoteModel vote)
        {
            int voterId = _dataAccess.IsVoterLoginValid(vote.Email, vote.HashedPw, vote.ElectionId);
            if (voterId == 0)
            {
                return Unauthorized();
            }
            var electionDetails = _dataAccess.GetElectionDetailsFromId(vote.ElectionId);
            var candidatesOfElection = _dataAccess.GetCandidatesOfElection(vote.ElectionId);
            if (electionDetails.IsCompleted == true || _dataAccess.IsUserVoted(vote.ElectionId, vote.Email))
            {
                return BadRequest("This election has completed/finished by admins OR already voted !");
            }
            vote.VoterId = voterId;
            List<int> candidateIds = new List<int>();
            foreach (var candidate in candidatesOfElection)
            {
                candidateIds.Add(candidate.Id);
            }
            if (candidateIds.Contains(Convert.ToInt32(vote.Vote)) == false)
            {
                return BadRequest("this candidate is not exist for the specified election ! vote dismissed !");
            }
            vote.Vote = RSAHelper.EncryptRSA(vote.Vote.ToString(),Keys.PubKey,"utf8");
            _dataAccess.InsertVote(vote);
            return Ok("vote sucess");
        }

        /// <summary>
        /// returns list of election which are the voter has defined to
        /// </summary>
        [HttpGet]
        public IActionResult GetElectionsOfVoterByEmail([FromQuery] string email)
        {
            List<ElectionModel> elections = new List<ElectionModel>();
            foreach (int id in _dataAccess.GetElectionsOfVoter(email))
            {
                elections.Add(_dataAccess.GetElectionDetailsFromId(id));
            }
            return Ok(elections);
        }


        [HttpGet]
        public IActionResult IsVoterVoteForElection([FromQuery] IsVoted isVoted)
        {
            bool result = _dataAccess.IsUserVoted(isVoted.electionId,isVoted.email);
            return Ok(new { isVoted=result.ToString()});
        }
    }
}