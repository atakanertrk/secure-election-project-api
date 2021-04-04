using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ElectionResultsModel
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public int Votes { get; set; }
    }
}
