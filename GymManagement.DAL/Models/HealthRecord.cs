using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weigth { get; set; }
        public string BloodType { get; set; } = default!;
        public string? Note { get; set; }
    }
}
