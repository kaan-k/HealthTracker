using System;

namespace Entities.Dtos
{
    public class WaterLogDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int AmountMl { get; set; }
    }
}
