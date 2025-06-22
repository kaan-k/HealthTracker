using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enums;
using Core.Constants;

namespace Entities
{
    public class Meal
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public MealType Type { get; set; }

        [MaxLength(ValidationConstants.MaxDescriptionLength)]
        public string Description { get; set; }

        [Range(ValidationConstants.MinCalories, ValidationConstants.MaxCalories)]
        public int Calories { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }
    }
}
