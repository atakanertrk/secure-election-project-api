using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessClassLibrary.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPw { get; set; }
        public bool IsEmailValidated { get; set; }
        public string VerificationCode { get; set; }
    }
}
