using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataAccessClassLibrary.EFModels
{
    public partial class VotersOfElection
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPw { get; set; }
        public int ElectionId { get; set; }
        public bool Voted { get; set; }

        public virtual Elections Election { get; set; }
    }
}
