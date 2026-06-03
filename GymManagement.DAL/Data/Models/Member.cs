using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; } 

        //Navigation properties
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Booking> Book { get; set; } = new List<Booking>();
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
