using FrogExhibitionBLL.DTO.FrogDTOs;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionBLL.ValidationAttributes
{
    public class CurrentAgeFrogAttribute : ValidationAttribute
    {
        /// <summary>
        /// Attribute checks that max age isn't lesser than current age  
        /// </summary>
        /// <param name="value">Frog object to validate</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            return value switch
            {
                null => true,
                FrogDtoForCreate frog => (frog?.CurrentAge != null && frog.CurrentAge <= frog.MaxAge),
                FrogDtoForUpdate frog => (frog?.CurrentAge != null && frog.CurrentAge <= frog.MaxAge),
                _ => false
            };
        }
    }
}
