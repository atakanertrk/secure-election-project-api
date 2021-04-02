using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class VotesModel
    {
        public int ElectionId { get; set; }
        public string Vote { get; set; }
    }
}
