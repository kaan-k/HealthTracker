using Entities.Enums;
using System;

namespace Entities.Dtos
{
    public class MealDto
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public MealType Type { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public DateTime Date { get; set; }
    }
}
