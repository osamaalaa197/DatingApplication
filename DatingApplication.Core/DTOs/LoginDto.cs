using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.DTOs
{
    public class LoginDto
    {
        [EmailAddress]
        public string Gmail { get; set; }
        public string Password { get; set; }
    }
}
