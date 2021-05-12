using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessClassLibrary.Models
{
    public class AddVoterToElectionModelDTO
    {
        public string Email { get; set; }
        public int ElectionId { get; set; }
    }
}
