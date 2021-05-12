using DataAccessClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CreateElectionModel
    {
        public ElectionModel electionDetails { get; set; }
        public List<CandidateModel> candidates { get; set; }
    }
}
