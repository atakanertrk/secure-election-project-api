using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class VoterModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPw { get; set; }
        public string ElectionId { get; set; }
        public bool Voted { get; set; }
    }
}
