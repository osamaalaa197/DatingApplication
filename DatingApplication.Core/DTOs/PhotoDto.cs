using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApplication.Core.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string? URL { get; set; }
        public bool IsMain { get; set; }

    }
}
