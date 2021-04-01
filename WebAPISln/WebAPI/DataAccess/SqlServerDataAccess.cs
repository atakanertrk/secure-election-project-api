using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DataAccess
{
    public class SqlServerDataAccess
    {
        private readonly string _conStr;
        public SqlServerDataAccess(IConfiguration config)
        {
            _conStr = config.GetConnectionString("SqlServerConnectionString");
        }
        /// <summary>
        /// returns admin id if login is valid,
        /// return 0 if login is not valid
        /// </summary>
        public int IsAdminLoginValid(string name, string hashedPw)
        {
            using (IDbConnection cnn = new SqlConnection(_conStr))
            {
                var p = new DynamicParameters();
                p.Add("@Name", name);
                p.Add("@HashedPw", hashedPw);

                string sql = "SELECT Id FROM Admins WHERE Name=@Name and HashedPw=@HashedPw";

                return cnn.Query<int>(sql, p).ToList().FirstOrDefault();
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
    }
}
