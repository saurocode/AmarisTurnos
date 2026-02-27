using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amaris.Application.DTOs.Turn
{
    public class TurnResponseDto
    {
        public int Id { get; set; }
        public string TurnCode { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public int IdLocation { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public DateTime DateExpiration { get; set; }
        public DateTime? DateActivation { get; set; }
        public string Status { get; set; } = string.Empty;
        public int MinutesRemaining { get; set; }
    }
}
