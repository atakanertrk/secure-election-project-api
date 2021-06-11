using Dapper;
using DataAccessClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessClassLibrary.DataAccess
{
    public class SqlServerDataAccess : IDataAccess
    {
        private readonly string _conStr;
        public SqlServerDataAccess(string conStr)
        {
            _conStr = conStr;
        }

        /// <summary>
        /// returns admin id if login is valid,
        /// return 0 if login is not valid
        /// </summary>
        public int IsAdminLoginValid(string email, string hashedPw)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                p.Add("@HashedPw", hashedPw);

                string sql = "SELECT Id FROM Admins WHERE Email=@Email and HashedPw=@HashedPw";

                return cnn.Query<int>(sql, p).ToList().FirstOrDefault();
            }
        }

        public void UpdateAdmin(AdminModel admin)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", admin.Email);
                p.Add("@HashedPw", admin.HashedPw);
                p.Add("@IsEmailValidated", admin.IsEmailValidated);
                p.Add("@VerificationCode", admin.VerificationCode);
                string sql = "UPDATE Admins SET HashedPw=@HashedPw, IsEmailValidated=@IsEmailValidated, VerificationCode=@VerificationCode WHERE Email=@Email;";

                cnn.Execute(sql, p);
            }
        }

        public AdminModel GetAdminDetailsByAdminId(string id)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Id", id);

                string sql = "SELECT * FROM Admins WHERE Id=@Id;";

                return cnn.Query<AdminModel>(sql, p).ToList().FirstOrDefault();
            }
        }

        public AdminModel GetAdminDetailsByAdminEmail(string email)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);

                string sql = "SELECT * FROM Admins WHERE Email=@Email;";

                return cnn.Query<AdminModel>(sql, p).FirstOrDefault();
            }
        }

        public void DeleteAdmin(string email)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);

                string sql = "DELETE FROM Admins WHERE Email=@Email;";
                cnn.Execute(sql,p);
            }
        }

        public void InsertNewAdmin(string email, string hashedPw)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                p.Add("@HashedPw", hashedPw);
                string sql = "INSERT INTO Admins (Email,HashedPw) VALUES (@Email,@HashedPw);";
                cnn.Execute(sql, p);
            }
        }

        public void InsertVoterToSpecifiedElection(AddVoterToElectionModel model)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", model.dtoModel.Email);
                p.Add("@ElectionId", model.dtoModel.ElectionId);
                p.Add("@HashedPw", model.HashedPw);
                string sql = "INSERT INTO VotersOfElection (Email,ElectionId,HashedPw) VALUES (@Email,@ElectionId,@HashedPw);";

                cnn.Execute(sql, p);
            }
        }
        public bool IsUserVoted(int electionId, string email)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                p.Add("@ElectionId", electionId);
                string sql = "SELECT Voted FROM VotersOfElection WHERE ElectionId=@ElectionId and Email=@Email;";
                return cnn.Query<bool>(sql, p).ToList().First();
            }
        }
        public string GetElectionNameFromId(int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Id", electionId);
                string sql = "SELECT Header FROM Elections WHERE Id=@Id;";

                return cnn.Query<string>(sql, p).ToList().FirstOrDefault();
            }
        }

        public ElectionModel GetElectionDetailsFromId(int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Id", electionId);
                string sql = "SELECT * FROM Elections WHERE Id=@Id;";

                return cnn.Query<ElectionModel>(sql, p).ToList().FirstOrDefault();
            }
        }

        public void InsertVote(VoteModel model)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@ElectionId", model.ElectionId);
                p.Add("@Vote", model.Vote);
                p.Add("@Voted", true);
                p.Add("@Id", model.VoterId);
                string sql = "INSERT INTO VotesOfElection (ElectionId,Vote) VALUES (@ElectionId,@Vote); UPDATE VotersOfElection SET Voted=@Voted WHERE Id=@Id";
                cnn.Execute(sql, p);
            }
        }
        public void DeleteVoterFromElection(int electionId, string email)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                p.Add("@ElectionId", electionId);
                string sql = "DELETE FROM VotersOfElection WHERE Email=@Email and ElectionId=@ElectionId;";

                cnn.Execute(sql, p);
            }
        }
        public int InsertElection(ElectionModel m)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@AdminId", m.AdminId);
                p.Add("@Description", m.Description);
                p.Add("@Header", m.Header);

                string sql = "INSERT INTO Elections (AdminId,Description,Header) VALUES (@AdminId,@Description,@Header); SELECT SCOPE_IDENTITY();";

                return cnn.Query<int>(sql, p).ToList().FirstOrDefault();
            }
        }

        public void InsertCandidateToElection(CandidateModel candidate)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Name", candidate.Name);
                p.Add("@Description", candidate.Description);
                p.Add("@ElectionId", candidate.ElectionId);

                string sql = "INSERT INTO Candidates (Name,Description,ElectionId) VALUES (@Name,@Description,@ElectionId);";

                cnn.Execute(sql, p);
            }
        }

        public void UpdateElectionStatus(bool status, int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@IsCompleted", status);
                p.Add("@Id", electionId);

                string sql = "UPDATE Elections SET IsCompleted=@IsCompleted WHERE Id=@Id;";

                cnn.Execute(sql, p);
            }
        }

        public List<CandidateModel> GetCandidatesOfElection(int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@ElectionId", electionId);

                string sql = "SELECT * FROM Candidates WHERE ElectionId=@ElectionId;";

                return cnn.Query<CandidateModel>(sql, p).ToList();
            }
        }

        public bool IsAdminCreatorOfSpecifiedElection(int adminId, int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@AdminId", adminId);
                p.Add("@Id", electionId);

                string sql = "SELECT COUNT(*) FROM Elections WHERE Id=@Id and AdminId=@AdminId;";

                int count = cnn.Query<int>(sql, p).ToList().FirstOrDefault();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<ElectionModel> GetAllElections()
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                string sql = "SELECT * FROM Elections;";

                return cnn.Query<ElectionModel>(sql).ToList();
            }
        }

        public List<string> GetVotersOfElection(int electionId)
        {
            var p = new DynamicParameters();
            p.Add("@ElectionId", electionId);
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                string sql = "SELECT Email FROM VotersOfElection WHERE ElectionId=@ElectionId;";

                return cnn.Query<string>(sql, p).ToList();
            }
        }

        public List<int> GetElectionsOfVoter(string email)
        {
            var p = new DynamicParameters();
            p.Add("@Email", email);
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                string sql = "SELECT ElectionId FROM VotersOfElection WHERE Email=@Email;";

                return cnn.Query<int>(sql, p).ToList();
            }
        }

        /// <summary>
        /// returns voterid if login is valid,
        /// return 0 if login is not valid
        /// </summary>
        public int IsVoterLoginValid(string email, string hashedPw, int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Email", email);
                p.Add("@HashedPw", hashedPw);
                p.Add("@ElectionId", electionId);

                string sql = "SELECT Id FROM VotersOfElection WHERE Email=@Email and HashedPw=@HashedPw and ElectionId=@ElectionId;";

                return cnn.Query<int>(sql, p).ToList().FirstOrDefault();
            }
        }

        public List<string> GetVotesOfElection(int electionId)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@ElectionId", electionId);
                string sql = "SELECT Vote FROM VotesOfElection WHERE ElectionId=@ElectionId;";

                return cnn.Query<string>(sql, p).ToList();
            }
        }



    }
}
