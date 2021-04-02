using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DeleteVoterFromElectionModel
    {
        public int ElectionId { get; set; }
        public string Email { get; set; }
    }
}
