using System.ComponentModel.DataAnnotations;

namespace FrogExhibitionDAL.ValidationAttributes
{
    public class ValidStrings : ValidationAttribute
    {
        
        private static string[] _strings;

        public ValidStrings(string[] strings)
        {
            _strings= strings;
        }
        /// <summary>
        /// Attribute checks that strings contain value 
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string str = value.ToString();
                return _strings.Contains(str);
            }
            return false;
        }
    }
}
