using DataAccessClassLibrary.EFModels;
using DataAccessClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessClassLibrary.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DataAccessClassLibrary.DataAccess
{
    public class EFSqlServerDataAccess : IDataAccess
    {
        private string _conStr { get; set; }
        private AutoMapper.IMapper _mapper;
        private ElectionDBContext _context;
        public EFSqlServerDataAccess(string conStr)
        {
            _mapper = new MapperHelper().GetMapper();
            _conStr = conStr;
            _context = new ElectionDBContext(_conStr);
        }
        public void DeleteVoterFromElection(int electionId, string email)
        {
            /*
              department = db.Departments.Where(d => d.Name == "Sales").First();
                db.Departments.Remove(department);
                db.SaveChanges();
             */
            var voterToBeDeleted = _context.VotersOfElection.Where(x => x.ElectionId == electionId && x.Email == email).First();
            _context.VotersOfElection.Remove(voterToBeDeleted);
            _context.SaveChanges();
            //VotersOfElection votersOfElection = new VotersOfElection { ElectionId = electionId, Email = email };
            //_context.VotersOfElection.Remove(votersOfElection);
            //_context.SaveChanges();

        }

        public AdminModel GetAdminDetailsByAdminId(string id)
        {
            var output = _context.Admins.Where(x => x.Id == Convert.ToInt32(id)).ToList();
            return _mapper.Map<AdminModel>(output);
        }

        public List<ElectionModel> GetAllElections()
        {
            var result = _context.Elections.ToList();
            var output = _mapper.Map<List<ElectionModel>>(result);
            return output;
        }

        public List<CandidateModel> GetCandidatesOfElection(int electionId)
        {
            var result = _context.Candidates.Where(x => x.ElectionId == electionId).ToList();
            return _mapper.Map<List<CandidateModel>>(result);
        }

        public ElectionModel GetElectionDetailsFromId(int electionId)
        {
            var result = _context.Elections.Where(x => x.Id == electionId).ToList().FirstOrDefault();
            return _mapper.Map<ElectionModel>(result);
        }

        public string GetElectionNameFromId(int electionId)
        {
            return _context.Elections.Where(x => x.Id == electionId).ToList().FirstOrDefault().Header;

        }

        public List<int> GetElectionsOfVoter(string email)
        {
            var result = _context.VotersOfElection.Where(x => x.Email == email).ToList();
            List<int> output = new List<int>();
            foreach (var electionId in result)
            {
                output.Add(electionId.ElectionId);
            }
            return output;
        }

        public List<string> GetVotersOfElection(int electionId)
        {
            var result = _context.VotersOfElection.Where(x => x.ElectionId == electionId).ToList();
            List<string> output = new List<string>();
            foreach (var item in result)
            {
                output.Add(item.Email);
            }
            return output;
        }

        public List<string> GetVotesOfElection(int electionId)
        {
            var result = _context.VotesOfElection.Where(x => x.ElectionId == electionId).ToList();
            List<string> output = new List<string>();
            foreach (var item in result)
            {
                output.Add(item.Vote);
            }
            return output;
        }

        public void InsertCandidateToElection(CandidateModel candidate)
        {
            var newCandidate = _mapper.Map<Candidates>(candidate);
            var result = _context.Candidates.Add(newCandidate);
            _context.SaveChanges();
        }

        public int InsertElection(ElectionModel m)
        {
            var newElection = _mapper.Map<Elections>(m);
            var result = _context.Elections.Add(newElection);
            _context.SaveChanges();
            return newElection.Id;
        }

        public void InsertNewAdmin(string name, string hashedPw)
        {
            Admins admin = new Admins { Name = name, HashedPw = hashedPw };
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public void InsertVote(VoteModel model)
        {
            VotesOfElection votesOfElection = new VotesOfElection { ElectionId = model.ElectionId, Vote = model.Vote };
            _context.VotesOfElection.Add(votesOfElection);
            var voterThatVotes = _context.VotersOfElection.Find(model.VoterId);
            voterThatVotes.Voted = true;
            _context.Entry(voterThatVotes).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            // _context.VotesOfElection.FromSqlInterpolated($"INSERT INTO VotesOfElection (ElectionId,Vote) VALUES ({model.ElectionId},{model.Vote}); UPDATE VotersOfElection SET Voted={true} WHERE Id={model.VoterId};");
            _context.SaveChanges();
        }

        public void InsertVoterToSpecifiedElection(AddVoterToElectionModel model)
        {
            var newVoterModel = new VotersOfElection { ElectionId = model.dtoModel.ElectionId, Email = model.dtoModel.Email, HashedPw = model.HashedPw };
            _context.VotersOfElection.Add(newVoterModel);
            _context.SaveChanges();
        }

        public bool IsAdminCreatorOfSpecifiedElection(int adminId, int electionId)
        {
            // SELECT COUNT(*) FROM Elections WHERE Id=@Id and AdminId=@AdminId
            var result = _context.Elections.Where(x => x.Id == electionId && x.AdminId == adminId).ToList().FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int IsAdminLoginValid(string name, string hashedPw)
        {
            var result = _context.Admins.Where(x => x.Name == name && x.HashedPw == hashedPw).ToList().FirstOrDefault();
            if (result == null)
            {
                return 0;
            }
            else
            {
                return result.Id;
            }
        }

        public bool IsUserVoted(int electionId, string email)
        {
            var result = _context.VotersOfElection.Where(x => x.Email == email && x.ElectionId == electionId).ToList().FirstOrDefault();
            return result == null ? throw new NullReferenceException("IsvoteRVoted is returned null which was not expected !") : result.Voted;
        }

        public int IsVoterLoginValid(string email, string hashedPw, int electionId)
        {
            var result = _context.VotersOfElection.Where(x => x.Email == email &&
                         x.HashedPw == hashedPw && x.ElectionId == electionId).ToList().FirstOrDefault();
            return result == null ? 0 : result.Id;
        }

        public void UpdateElectionStatus(bool status, int electionId)
        {
            var election = _context.Elections.Find(electionId);
            election.IsCompleted = status;
            _context.Entry(election).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}