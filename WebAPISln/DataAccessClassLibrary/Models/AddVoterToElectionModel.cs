using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessClassLibrary.Models
{
    public class AddVoterToElectionModel
    {
        public string HashedPw { get; set; }
        public AddVoterToElectionModelDTO dtoModel { get; set; }
    }
}
