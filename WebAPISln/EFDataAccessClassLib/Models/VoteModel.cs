using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAccessClassLib.Models
{
    public class VoteModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int ElectionId { get; set; }
        public string Vote { get; set; }
        public int VoterId { get; set; }
    }
}
