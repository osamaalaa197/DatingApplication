using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.EF.Helper
{
    public class LikeParams : PaginationParams
    {
        public string UserId { get; set; }
    }
}
