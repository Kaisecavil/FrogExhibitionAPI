using FrogExhibitionDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.ValidationAttributes
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
                Frog frog => frog?.CurrentAge != null && frog.CurrentAge <= frog.MaxAge,
                _ => false
            };
        }
    }
}
