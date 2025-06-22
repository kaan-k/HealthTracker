using Core.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WeightLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        [Range(ValidationConstants.MinWeightKg, ValidationConstants.MaxWeightKg)]
        public double WeightKg { get; set; }
        public User User { get; set; }
    }
}

