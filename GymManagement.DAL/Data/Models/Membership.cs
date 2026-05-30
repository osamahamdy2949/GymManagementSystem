using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Membership : BaseEntity
    {
        //StartDate = CreatedAt
        public DateOnly? EndDate { get; set; }
        public string Status => EndDate > DateOnly.FromDateTime(DateTime.Now) ? "Active" : "Expired";


        //Navigation Properties
        public Member Member { get; set; } = default!;
        public Plan Plan { get; set; } = default!;
        //Foreign Keys
        public int MemberId { get; set; }
        public int PlanId { get; set; }

        [NotMapped]
        public bool IsActive => EndDate > DateOnly.FromDateTime(DateTime.Now);
    }
}
