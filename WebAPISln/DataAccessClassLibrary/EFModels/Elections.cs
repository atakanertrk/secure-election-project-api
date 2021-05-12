using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataAccessClassLibrary.EFModels
{
    public partial class Elections
    {
        public Elections()
        {
            Candidates = new HashSet<Candidates>();
            VotersOfElection = new HashSet<VotersOfElection>();
            VotesOfElection = new HashSet<VotesOfElection>();
        }

        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public int AdminId { get; set; }
        public bool IsCompleted { get; set; }

        public virtual Admins Admin { get; set; }
        public virtual ICollection<Candidates> Candidates { get; set; }
        public virtual ICollection<VotersOfElection> VotersOfElection { get; set; }
        public virtual ICollection<VotesOfElection> VotesOfElection { get; set; }
    }
}
