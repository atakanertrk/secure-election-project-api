using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;
using DataAccessClassLibrary.DataAccess;
using DataAccessClassLibrary.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class AdminController : ControllerBase
    {
        private IDataAccess _dataAccess;
        private TokenHelper _token;
        private string _emailPw;
        public AdminController(IConfiguration config)
        {
            _dataAccess = new SqlServerDataAccess(config.GetConnectionString("SqlServerConnectionString"));
            _token = new TokenHelper(config);
            _emailPw = config.GetValue<string>("Password:EmailPw");
        }

        private int GetIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            return Convert.ToInt32(claim[0].Value);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AdminLoginModel model)
        {
            string hashedPw = HashingHelper.EncryptSHA256(model.Password);
            int adminId = _dataAccess.IsAdminLoginValid(model.Email, hashedPw);
            if (adminId == 0) return Unauthorized();
            var admin = _dataAccess.GetAdminDetailsByAdminId(adminId.ToString());
            if (admin.IsEmailValidated == false) 
                return BadRequest("please verify your email before login");
            return Ok(new { token = _token.GenerateJSONWebToken(adminId, true) });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateAccount([FromBody] AdminLoginModel model)
        {
            string hashedPw = HashingHelper.EncryptSHA256(model.Password);
            _dataAccess.InsertNewAdmin(model.Email,hashedPw);
            try
            {
                string code = RSAHelper.EncryptRSA(model.Email,Keys.PubKey, "utf8");
                SendEmailModel send = new SendEmailModel() { Subject = "Verification Code For Admin",Body=code,To=model.Email };
                EmailHelper.Send(send,_emailPw);
            }
            catch (Exception)
            {
                _dataAccess.DeleteAdmin(model.Email);
                throw;
            }
            return Ok("created admin account, verify your email before login");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult VerifyEmail([FromBody] VerifyEmailModel model)
        {
            var admin = _dataAccess.GetAdminDetailsByAdminEmail(model.Email);
            // decrypted code should be equal to Email address
            string decryptedCode = RSAHelper.DecryptRSA(model.Code,Keys.PrivKey,"utf8");
            if(decryptedCode == admin.Email)
            {
                admin.IsEmailValidated = true;
                _dataAccess.UpdateAdmin(admin);
                return Ok("email verified");
            }
            return BadRequest("verification code is not valid");
        }


        /// <summary>
        /// create new election with information of election description and candidates information 
        /// (cannot be updated after creation)
        /// (you can add voters after creation)
        /// </summary>
        [HttpPut]
        public IActionResult CreateNewElection([FromBody] CreateElectionModel election)
        {
            if (election.candidates.Count <=0)
            {
                return BadRequest("no candidate included to the election");
            }
            int adminId = GetIdFromToken();
            election.electionDetails.AdminId = adminId;
            if (election.electionDetails.Description != null && election.electionDetails.Description.Length > 3 && election.electionDetails.Header != null && election.electionDetails.Header.Length > 3)
            {
                ElectionModel electionModel = election.electionDetails;
                int insertedElectionId = _dataAccess.InsertElection(election.electionDetails);
                foreach (var candidate in election.candidates)
                {
                    candidate.ElectionId = insertedElectionId;
                    _dataAccess.InsertCandidateToElection(candidate);
                }
                return Ok();
            }
            return BadRequest("description is too short or null");
        }

        [HttpPost]
        public IActionResult CompleteTheElection([FromQuery] int electionId)
        {
            int adminId = GetIdFromToken();
            if (_dataAccess.IsAdminCreatorOfSpecifiedElection(adminId,electionId))
            {
                _dataAccess.UpdateElectionStatus(true,electionId);
                return Ok();
            }
            return BadRequest("you are not the creator of the specified election, dont trick us !");
        }

        /// <summary>
        /// send voter email for the specified election
        /// email and hashed password will automatically send to added voter's mail adress manually
        /// </summary>
        [HttpPut]
        public IActionResult AddVoterToElection([FromBody] AddVoterToElectionModelDTO addModel)
        {
            int adminId = GetIdFromToken();
            
            if (_dataAccess.IsAdminCreatorOfSpecifiedElection(adminId, addModel.ElectionId))
            {
                if (_dataAccess.GetElectionDetailsFromId(addModel.ElectionId).IsCompleted == true)
                {
                    return BadRequest("Election completed/finished, cannot add new voter !");
                }
                string plainPw = RandomPassword.GenerateRandomPassword();
                string hashedPw = HashingHelper.EncryptSHA256(plainPw);
                try
                {
                    _dataAccess.InsertVoterToSpecifiedElection(new AddVoterToElectionModel() { dtoModel = addModel, HashedPw = hashedPw });
                }
                catch (Exception ex)
                {
                    var exception = ex;
                    return BadRequest("cannot add voter to election, voter might already added to the election");
                }
                SendEmailModel model = new SendEmailModel { Subject = $"Your Password For Election {_dataAccess.GetElectionNameFromId(addModel.ElectionId)}", Body = plainPw, To = addModel.Email };
                bool isSendSuccess;
                try
                {
                    isSendSuccess = EmailHelper.Send(model,_emailPw);
                }
                catch (Exception)
                {
                    _dataAccess.DeleteVoterFromElection(addModel.ElectionId, addModel.Email);
                    throw;
                }
                if (!isSendSuccess)
                {
                    _dataAccess.DeleteVoterFromElection(addModel.ElectionId,addModel.Email);
                    return BadRequest("email cannot send, failed adding voter to election");
                }
                return Ok("voter added and password sended to specified email adress");
            }
            return BadRequest("you are not admin of the specified election");
        }

        /// <summary>
        /// delete specified voter from the election if voter didnt vote yet
        /// </summary>
        [HttpDelete]
        public IActionResult DeleteVoterFromElection([FromBody] DeleteVoterFromElectionModel model)
        {
            if (_dataAccess.IsAdminCreatorOfSpecifiedElection(GetIdFromToken(), model.ElectionId) && _dataAccess.IsUserVoted(model.ElectionId,model.Email) == false)
            {
                _dataAccess.DeleteVoterFromElection(model.ElectionId,model.Email);
                return Ok();
            }
            return BadRequest("you are not admin of the specified election (or user that trying to delete is voted for election and cannot be deleted)");
        }

        /// <summary>
        /// returns list of election which are created by the admin
        /// </summary>
        [HttpGet]
        public IActionResult GetCreatedElections()
        {
            var electionsCreatedByAdmin = _dataAccess.GetAllElections().Where(x => x.AdminId == GetIdFromToken());
            return Ok(electionsCreatedByAdmin);
        }

        /// <summary>
        /// returns list of voter for specified election
        /// </summary>
        [HttpGet]
        public IActionResult GetVotersOfElection([FromQuery] int electionId)
        {
            return Ok(_dataAccess.GetVotersOfElection(electionId));
        }
    }
}
