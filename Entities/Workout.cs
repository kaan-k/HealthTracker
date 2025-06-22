using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Workout
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required, MaxLength(1000)]
        public string Plan { get; set; }
    }
}
