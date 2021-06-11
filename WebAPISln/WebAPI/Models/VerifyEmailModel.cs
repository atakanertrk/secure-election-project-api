using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class VerifyEmailModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
