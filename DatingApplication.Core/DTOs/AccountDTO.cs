using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string? PhotoUrl { get; set; }
        public string? KnownAs { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastActive { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public List<PhotoDto>? Photos { get; set; }
    }
}
