using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.DataAccess
{
    public interface IDataAccess
    {
        void DeleteVoterFromElection(int electionId, string email);
        AdminModel GetAdminDetailsByAdminId(string id);
        List<ElectionModel> GetAllElections();
        List<CandidateModel> GetCandidatesOfElection(int electionId);
        ElectionModel GetElectionDetailsFromId(int electionId);
        string GetElectionNameFromId(int electionId);
        List<int> GetElectionsOfVoter(string email);
        List<string> GetVotersOfElection(int electionId);
        List<string> GetVotesOfElection(int electionId);
        void InsertCandidateToElection(CandidateModel candidate);
        int InsertElection(ElectionModel m);
        void InsertVote(VoteModel model);
        void InsertVoterToSpecifiedElection(AddVoterToElectionModel model);
        bool IsAdminCreatorOfSpecifiedElection(int adminId, int electionId);
        int IsAdminLoginValid(string name, string hashedPw);
        bool IsUserVoted(int electionId, string email);
        int IsVoterLoginValid(string email, string hashedPw, int electionId);
        void UpdateElectionStatus(bool status, int electionId);
    }
}