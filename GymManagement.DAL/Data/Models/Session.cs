using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Navigation properties
        public Category Category { get; set; } =default!;
        public Trainer Trainer { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        // Foreign keys
        public int CategoryId { get; set; }
        public int TrainerId { get; set; }
    }
}
