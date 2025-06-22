using System;

namespace Entities.Dtos
{
    public class WorkoutDto
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public DateTime Date { get; set; }
        public string Plan { get; set; }
    }
}
