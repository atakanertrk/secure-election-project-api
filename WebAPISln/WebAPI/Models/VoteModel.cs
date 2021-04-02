using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class VoteModel
    {
        public string Email { get; set; }
        public string HashedPw { get; set; }
        public int ElectionId { get; set; }
        public string Vote { get; set; }
        public int VoterId { get; set; }
    }
}
