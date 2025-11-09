using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.Models
{
    public class UserLike
    {
        public int Id { get; set; }
        public string SourceUserId { get; set; }
        public string TargetUserId {  get; set; }
        public ApplicationUser TargetUser { get; set; }

    }
}
