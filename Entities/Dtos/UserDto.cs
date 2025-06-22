namespace Entities.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public double HeightCm { get; set; }
        public double CurrentWeightKg { get; set; }
        public double TargetWeightKg { get; set; }
    }
}
