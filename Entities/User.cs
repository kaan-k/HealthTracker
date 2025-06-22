using Core.Constants;
using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    public string Gender{ get; set; }


    [MaxLength(ValidationConstants.MaxFullNameLength)]
    public string FullName { get; set; }

    [Range(ValidationConstants.MinAge, ValidationConstants.MaxAge)]
    public int Age { get; set; }

    [Range(ValidationConstants.MinHeightCm, ValidationConstants.MaxHeightCm)]
    public double HeightCm { get; set; }

    [Range(ValidationConstants.MinWeightKg, ValidationConstants.MaxWeightKg)]
    public double CurrentWeightKg { get; set; }

    [Range(ValidationConstants.MinWeightKg, ValidationConstants.MaxWeightKg)]
    public double TargetWeightKg { get; set; }
}
