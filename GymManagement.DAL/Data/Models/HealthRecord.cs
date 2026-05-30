using GymManagement.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public BloodType BloodType { get; set; }
        public string? Note { get; set; }

        //Navigation properties
        public Member Member { get; set; } = default!;
        // Foreign key for Member
        public int MemberId { get; set; }
    }
}
