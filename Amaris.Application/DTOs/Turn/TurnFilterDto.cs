using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaris.Application.DTOs.Turn
{
    public class TurnFilterDto
    {
        public string? Identification { get; set; }
        public string? Status { get; set; }
        public int? LocationId { get; set; }
        public int? ServiceId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
