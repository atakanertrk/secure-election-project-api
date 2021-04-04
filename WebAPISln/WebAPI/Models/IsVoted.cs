using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class IsVoted
    {
        public int electionId { get; set; }
        public string email { get; set; }
    }
}
