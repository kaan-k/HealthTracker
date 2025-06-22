using System.ComponentModel.DataAnnotations;

namespace Core.Utilities.Validation
{
    public static class ValidationHelper
    {
        public static void ValidateObject<T>(T obj)
        {
            var context = new ValidationContext(obj, null, null);
            Validator.ValidateObject(obj, context, validateAllProperties: true);
        }
    }
}
