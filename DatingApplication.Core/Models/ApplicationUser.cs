using DatingApplication.API.Extentions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirthday { get; set; }
        public string? KnownAs { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public DateTime LastActive {  get; set; }=DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
        public string Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual List<UserLike> UserLikes { get; set; }
        public List<Photo> Photos { get; set; } = [];

        public int GetAge()
        {
            return DateOfBirthday.CalculateAge();
        }
    }
}
