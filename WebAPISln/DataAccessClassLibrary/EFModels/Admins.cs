using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataAccessClassLibrary.EFModels
{
    public partial class Admins
    {
        public Admins()
        {
            Elections = new HashSet<Elections>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string HashedPw { get; set; }

        public virtual ICollection<Elections> Elections { get; set; }
    }
}
