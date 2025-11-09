using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
