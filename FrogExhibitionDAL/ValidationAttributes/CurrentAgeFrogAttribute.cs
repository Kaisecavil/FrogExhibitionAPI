using FrogExhibitionDAL.Models;
using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.ValidationAttributes
{
    public class CurrentAgeFrogAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            Frog f = value as Frog;
            try
            {
                if (f.MaxAge < f.CurrentAge)
                {
                    return false;
                }
            }
            catch(NullReferenceException ex) 
            {
                return false; // можно ли так или нужно выкидывать наверх? 
            }
            return true;
        }
    }
}
