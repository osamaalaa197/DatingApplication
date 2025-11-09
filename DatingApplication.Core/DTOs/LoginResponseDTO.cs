using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.DTOs
{
    public class LoginResponseDTO
    {
        public string UserId {  get; set; }
        public string Token { get; set; }
        public string? PhotoUrl { get; set; }
        public string UserName { get; set; }
    }
}
