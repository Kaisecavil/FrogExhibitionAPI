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
            if (value == null)
            {
                return true;
            }
            if(value is Frog)
            {
                Frog frog = value as Frog;
                try
                {
                    if (frog.CurrentAge <= frog.MaxAge)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (NullReferenceException ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
    }
}
