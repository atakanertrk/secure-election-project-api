using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessClassLibrary.Models
{
    public class ElectionModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public int AdminId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
