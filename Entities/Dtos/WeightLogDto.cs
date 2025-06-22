using System;

namespace Entities.Dtos
{
    public class WeightLogDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public double WeightKg { get; set; }
        public DateTime Date { get; set; }
    }
}
